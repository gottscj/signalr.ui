<template>
  <div id="ApiClientMethod">
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
            'clientmethod-name-expanded': expanded,
            'clientmethod-name-collapsed': !expanded,
          }"
        >
          {{ clientMethod.name }}
        </div>
        <div class="clientmethod-description">
          {{ clientMethod.description }}
        </div>
        <BFormCheckbox v-model="listening" class="clientmethod-enable" switch />
      </button>
      <div v-if="expanded" class="accordion-panel">
        <div class="clientmethod-subtitle">Parameters definition</div>
        <div class="clientmethod-definition">
          {{ clientMethod.params }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { BFormCheckbox } from "bootstrap-vue";
import { HubConnection } from "@microsoft/signalr";

export default {
  name: "ApiClientMethod",
  components: {
    BFormCheckbox,
  },
  props: {
    value: {
      type: Boolean,
    },
    clientMethod: {
      name: String,
      description: String,
      params: {},
    },
    connection: HubConnection,
    color: String,
    hubName: String,
  },
  emits: ["input"],
  data: function () {
    return {
      expanded: false,
      listening: this.value,
    };
  },
  watch: {
    connection: function () {
      this.updateClientMethodListening();
    },
    listening: function () {
      this.updateClientMethodListening();
      this.$emit("input", this.listening);
    },
    value: function () {
      this.listening = this.value;
    },
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
    },
    updateClientMethodListening() {
      if (this.connection) {
        if (this.listening) {
          var func = (eventArgs) => {
            this.$root.$data.ClientMethodsService.add(
              this.color,
              this.hubName,
              this.clientMethod.name,
              JSON.stringify(eventArgs, null, 2)
            );
          };
          this.connection.on(this.clientMethod.name, function (eventArgs) {
            func(eventArgs);
          });
        } else {
          this.connection.off(this.clientMethod.name);
        }
      }
    },
  },
};
</script>

<style scoped lang="scss">
#ApiClientMethod {
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
    background-color: map-get($accent-color, 30);
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
    border-color: map-get($accent-color, 400);
    box-shadow: 3px 3px 3px 0 map-get($accent-color, 100),
      3px 3px 3px 0 map-get($accent-color, 100);
  }

  .accordion-top-collapsed {
    @extend .accordion-top;
    border-radius: 5px;
    border-color: map-get($accent-color, 300);
    box-shadow: 2px 2px 2px 0 map-get($accent-color, 50),
      2px 2px 2px 0 map-get($accent-color, 50);
  }

  .accordion-panel {
    font-family: inherit;
    display: block;
    background-color: map-get($accent-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($accent-color, 400);
    border-width: 1px;
    border-top: none;
    box-shadow: 3px 3px 3px 0 map-get($accent-color, 100),
      3px 3px 3px 0 map-get($accent-color, 100);
  }

  .clientmethod-name {
    color: $light-text;
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-width: 1px;
  }

  .clientmethod-name-expanded {
    @extend .clientmethod-name;
    border-color: map-get($accent-color, 700);
    background-color: map-get($accent-color, 700);
  }

  .clientmethod-name-collapsed {
    @extend .clientmethod-name;
    border-color: map-get($accent-color, 400);
    background-color: map-get($accent-color, 400);
  }

  .clientmethod-description {
    padding: 7px;
  }

  .clientmethod-enable {
    padding: 7px;
    margin-left: auto;
    margin-right: 0;
  }

  .clientmethod-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($primary-color, 900);
    background: map-get($accent-color, 50);
  }

  .clientmethod-definition {
    margin: 10px;
    font-size: 14px;
    white-space: pre-wrap;
    color: map-get($primary-color, 900);
  }
}
</style>
