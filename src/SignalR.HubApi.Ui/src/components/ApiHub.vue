<template>
  <div id="ApiHub">
    <button class="accordion" @click="toggleAccordion">
      <div class="hub-color" :style="{ background: color }" />
      <div class="hub-path">
        {{ hub.path }}
      </div>
      <div class="hub-name">
        {{ hub.name }}
      </div>
      <div class="hub-description">
        {{ hub.description }}
      </div>
      <BBadge v-if="this.connectionStatus === 'connected'" class="hub-online">
        {{ this.connectionStatus }}
      </BBadge>
      <BFormCheckbox
        v-model="connectHub"
        class="hub-connection"
        switch
        size="lg"
      />
      <div v-if="expanded" class="hub-arrow">
        <b-icon class="hub-arrow-icon" icon="chevron-down" aria-hidden="true" />
      </div>
      <div v-else class="hub-arrow">
        <b-icon
          class="hub-arrow-icon"
          icon="chevron-right"
          aria-hidden="true"
        />
      </div>
    </button>
    <div class="panel" :style="{ display: panelDisplay }">
      <div v-if="hub.methods.length > 0" class="hub-group">
        <div class="hub-group-title">Methods</div>
        <div v-for="method in hub.methods" :key="method.name">
          <ApiMethod
            class="hub-group-element"
            :connection="connection"
            :method="method"
          />
        </div>
      </div>

      <div v-if="hub.clientMethods.length > 0" class="hub-group">
        <div class="hub-group-title">
          <BFormCheckbox v-model="enableAllClientMethods" switch size="lg">
            Client Methods
          </BFormCheckbox>
        </div>
        <div
          v-for="(clientMethod, index) in hub.clientMethods"
          :key="clientMethod.name"
        >
          <ApiClientMethod
            v-model="clientMethodsState[index]"
            class="hub-group-element"
            :connection="connection"
            :client-method="clientMethod"
            :color="color"
            :hub-name="hub.name"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiMethod from "./ApiMethod.vue";
import ApiClientMethod from "./ApiClientMethod.vue";
import { BBadge, BFormCheckbox, BIcon } from "bootstrap-vue";
import { HubConnectionBuilder } from "@microsoft/signalr";

const ConnectionStatus = {
  Connected: "connected",
  Disconnected: "disconnected",
};

export default {
  name: "ApiHub",
  components: {
    ApiMethod,
    ApiClientMethod,
    BBadge,
    BFormCheckbox,
    BIcon,
  },
  data: function () {
    return {
      expanded: true,
      panelDisplay: "block",
      connectHub: false,
      connectionStatus: ConnectionStatus.Disconnected,
      connectionError: "",
      connection: void 0,
      clientMethodsState: [],
      enableAllClientMethods: true,
    };
  },
  watch: {
    enableAllClientMethods: function () {
      if (this.allClientMethodsEnabled != this.enableAllClientMethods) {
        this.clientMethodsState = Array(this.hub.clientMethods.length).fill(
          this.enableAllClientMethods
        );
      }
    },
    allClientMethodsEnabled: function () {
      this.enableAllClientMethods = this.allClientMethodsEnabled;
    },
    connectHub: function () {
      if (
        (this.connectHub === true &&
          !this.connection &&
          this.connectionStatus !== ConnectionStatus.Connected) ||
        (this.connectHub === false &&
          this.connection &&
          this.connectionStatus !== ConnectionStatus.Disconnected)
      ) {
        this.toggleHubConnection();
      }
    },
  },
  props: {
    color: String,
    serverInfo: void 0,
    hub: {
      name: String,
      path: String,
      description: String,
      methods: [],
      clientMethods: [],
    },
  },
  created() {
    this.clientMethodsState = Array(this.hub.clientMethods.length).fill(
      this.enableAllClientMethods
    );
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.panelDisplay = this.expanded ? "block" : "none";
    },
    async toggleHubConnection() {
      if (this.connectionStatus === ConnectionStatus.Disconnected) {
        await this.connect();
      } else {
        this.disconnect();
      }
    },
    async connect() {
      console.log(`Trying to connect ${this.hubPath}`);
      this.connection = new HubConnectionBuilder()
        .withUrl("https://localhost:5001/chat")
        .build();

      try {
        await this.connection.start();
        this.connectionStatus = ConnectionStatus.Connected;
        this.connectHub = true;

        this.connection.onclose(async (error) => {
          if (error) {
            console.error(error);
          }
          this.connectionStatus = ConnectionStatus.Disconnected;
          this.connectHub = false;
        });
      } catch (error) {
        console.error(error);
        this.disconnect();
      }
    },
    async disconnect() {
      await this.connection.stop();
      this.connection = void 0;
      this.connectionStatus = ConnectionStatus.Disconnected;
      this.connectHub = false;
    },
  },
  computed: {
    hubPath: function () {
      return this.serverInfo.url + this.hub.path;
    },
    allClientMethodsEnabled: function () {
      return this.clientMethodsState.every((x) => x === true);
    },
  },
};
</script>

<style scoped lang="scss">
#ApiHub {
  color: var(--primary-text);
  min-width: 600px;
  margin-bottom: 20px;

  .accordion {
    padding: 0px;
    font-family: inherit;
    background-color: var(--apihub-background);
    color: inherit;
    cursor: pointer;
    height: 60px;
    width: 100%;
    border: none;
    text-align: left;
    vertical-align: middle;
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-bottom: 1px solid var(--primary-border);
    border-radius: 7px 0px 0px 7px;
  }

  .panel {
    font-family: inherit;
    padding-left: 10px;
    overflow: hidden;
  }

  .hub-color {
    height: 59px;
    width: 10px;
    border-radius: 7px 0px 0px 7px;
  }

  .hub-connection {
    padding: 15px 5px 0px 0px;
    margin-right: 0px;
    margin-left: auto;
  }

  .hub-online {
    text-align: center;
    margin: 20px 0px 0px 20px;
    color: var(--apihub-hub-online-badge-text);
    background-color: var(--apihub-hub-online-badge-background);
  }

  .hub-name {
    height: 40px;
    line-height: 60px;
    text-align: center;
    color: inherit;
    padding: 0px 20px 0px 20px;
    font-size: 25px;
  }

  .hub-description {
    height: 40px;
    line-height: 60px;
    text-align: center;
    color: inherit;
    font-size: 15px;
  }

  .hub-path {
    color: var(--apihub-hub-path-text);
    background-color: var(--apihub-hub-path-background);
    font-size: 15px;
    margin-top: 10px;
    margin-left: 10px;
    padding: 7px;
    border-radius: 5px;
    border-style: solid;
    border-color: var(--apihub-hub-path-background);
    border-width: 1px;
  }

  .hub-arrow {
    height: 40px;
    line-height: 60px;
    margin-right: 10px;
    margin-left: 10px;
  }

  .hub-arrow-icon {
    color: var(--primary-text);
    font-weight: bold;
  }

  .hub-group {
    padding: 10px 10px 0px 10px;
  }

  .hub-group-title {
    font-size: 20px;
  }

  .hub-group-element {
    padding: 8px;
  }
}
</style>
