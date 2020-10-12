<template>
  <div id="ApiMethod">
    <div
      :class="{
        'accordion-expanded': expanded,
        'accordion-collapsed': !expanded,
      }"
    >
      <button
        :class="{
          'accordion-top-expanded': expanded,
          'accordion-top-collapsed': !expanded,
        }"
        @click="toggleAccordion"
      >
        <div
          :class="{
            'method-name-expanded': expanded,
            'method-name-collapsed': !expanded,
          }"
        >
          {{ method.name }}
        </div>
        <div class="method-description">
          {{ method.description }}
        </div>
      </button>
      <div v-show="expanded" class="accordion-panel">
        <div class="method-subtitle">Parameters</div>
        <div class="method-parameters">
          <ApiMethodParameters
            :parameters="method.params"
            @parameters-changed="onParametersChanged"
          />
          <ActionButtonWithStatus
            text="Try me!"
            hover-text="Go!"
            :status="callStatus"
            :status-text="callStatusText"
            @click="callMethod"
          />
        </div>
        <!--TODO: create json text area component shared among parameters and response -->
        <div class="method-subtitle">Response</div>
        <div class="hub-response">
          <textarea
            v-model="hubResponse"
            readonly
            :class="
              !hubResponseError ? 'hub-response-ok' : 'hub-response-error'
            "
            placeholder="JSON response will show here"
            :rows="hubResponseRows"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiMethodParameters from "./ApiMethodParameters.vue";
import ActionButtonWithStatus from "./ActionButtonWithStatus.vue";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";

export default {
  name: "ApiMethod",
  components: {
    ApiMethodParameters,
    ActionButtonWithStatus,
  },
  props: {
    connection: HubConnection,
    method: {
      name: String,
      description: String,
      response: Object,
      parameters: Array,
    },
  },
  data: function () {
    return {
      expanded: false,
      parametersJson: "",
      hubResponseOk: null,
      hubResponseError: null,
      callInProgress: false,
      callStatus: "none",
      callStatusText: null,
    };
  },
  computed: {
    hubResponse: function () {
      const response = this.hubResponseError
        ? this.hubResponseError
        : this.hubResponseOk;
      return response ? response : "";
    },
    hubResponseRows: function () {
      if (!this.hubResponse) {
        return 1;
      }
      return this.hubResponse.split("\n").length;
    },
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
    },
    onParametersChanged(value) {
      this.parametersJson = value;
    },
    async callMethod() {
      if (this.callInProgress) return;

      this.callInProgress = true;
      this.callStatus = "none";
      this.callStatusText = null;

      var timeout = setTimeout(() => {
        this.callStatus = "loading";
      }, 1000);

      if (
        !this.connection ||
        this.connection.state !== HubConnectionState.Connected
      ) {
        clearTimeout(timeout);
        this.callInProgress = false;
        this.callStatus = "error";
        this.callStatusText = "The hub must be connected";
        this.hubResponseOk = null;
        this.hubResponseError = null;
        return;
      }

      const parameters = this.processMethodParameters(this.parametersJson);

      this.connection
        .invoke(this.method.name, ...parameters)
        .then((response) => {
          this.hubResponseOk = JSON.stringify(response, null, 2);
          this.hubResponseError = null;
          clearTimeout(timeout);
          this.callInProgress = false;
          this.callStatus = "ok";
        })
        .catch((error) => {
          this.hubResponseError = JSON.stringify(error, null, 2);
          this.hubResponseOk = null;
          clearTimeout(timeout);
          this.callInProgress = false;
          this.callStatus = "ok";
        });
    },
    processMethodParameters(parametersJson) {
      const parametersArray = [];
      for (const param of parametersJson) {
        const values = Object.values(param);
        if (values.length != 1) {
          console.log("Invalid parameter has more than 1 value", param);
          return void 0;
        }
        parametersArray.push(values[0]);
      }
      return parametersArray;
    },
  },
};
</script>

<style scoped lang="scss">
#ApiMethod {
  color: map-get($primary-color, 400);

  .accordion-collapsed {
    transition: all 0.2s linear;
  }

  .accordion-expanded {
    margin-left: -8px;
    margin-right: 8px;
    transition: all 0.2s linear;
  }

  .accordion-top {
    font-family: inherit;
    background-color: map-get($secondary-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 5px;
    width: 100%;
    text-align: left;
    outline: none;
    font-size: 15px;
    display: flex;
    border-style: solid;
    border-width: 1px;
  }

  .accordion-top-expanded {
    @extend .accordion-top;
    border-radius: 5px 5px 0px 0px;
    border-color: map-get($secondary-color, 400);
    box-shadow: 3px 3px 3px 0 map-get($secondary-color, 100),
      3px 3px 3px 0 map-get($secondary-color, 100);
  }

  .accordion-top-collapsed {
    @extend .accordion-top;
    border-radius: 5px;
    border-color: map-get($secondary-color, A200);
    box-shadow: 2px 2px 2px 0 map-get($secondary-color, 50),
      2px 2px 2px 0 map-get($secondary-color, 50);
  }

  .accordion-panel {
    font-family: inherit;
    display: block;
    background-color: map-get($secondary-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($secondary-color, 400);
    border-width: 1px;
    border-top: none;
    box-shadow: 3px 3px 3px 0 map-get($secondary-color, 100),
      3px 3px 3px 0 map-get($secondary-color, 100);
  }

  .method-name {
    color: $light-text;
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-width: 1px;
  }

  .method-name-expanded {
    @extend .method-name;
    border-color: map-get($secondary-color, 800);
    background-color: map-get($secondary-color, 800);
  }

  .method-name-collapsed {
    @extend .method-name;
    border-color: map-get($secondary-color, 500);
    background-color: map-get($secondary-color, 500);
  }

  .method-description {
    padding: 7px;
  }

  .method-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($primary-color, 900);
    background: map-get($secondary-color, 50);
  }

  .method-parameters {
    margin: 10px;
  }

  .hub-response {
    margin: 10px;
  }

  .hub-response-error {
    width: 800px;
    font-size: 14px;
    border-color: $error-color;
    color: $error-color;
  }

  .hub-response-ok {
    width: 800px;
    font-size: 14px;
    border-color: map-get($secondary-color, A200);
  }
}
</style>
