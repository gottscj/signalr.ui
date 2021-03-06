<template>
  <div id="SignalRUi">
    <BNavbar class="navBar">
      <BNavbarBrand class="navBarTitle">
        <img class="logo" src="./assets/logo.png" alt="SignalR.UI" />
        SignalR.UI
      </BNavbarBrand>

      <!-- search box is disabled, because the functionality isn't implemented yet -->
      <BNavbarNav v-if="apiInfo !== void 0 && false" class="ml-auto">
        <SearchBox v-model="searchString" placeholder="Search for a method" />
      </BNavbarNav>

      <BNavbarNav v-if="!isSelfHosted" class="ml-auto">
        <BInputGroup prepend="Server" size="sm">
          <BFormSelect
            v-model="selectedServer"
            right
            size="sm"
            :options="selectServerOptions"
            @change="selectServer"
          />
          <BInputGroupAppend>
            <BButton @click="showAddServerDialog = !showAddServerDialog">
              +
            </BButton>
            <BButton
              v-if="
                selectedServerInfo !== void 0 &&
                selectedServerInfo.origin === 'local'
              "
              :style="{ 'background-color': '#c46464' }"
              @click="removeSelectedServer"
            >
              -
            </BButton>
          </BInputGroupAppend>
        </BInputGroup>
      </BNavbarNav>

      <BNavbarNav>
        <ThemeToggleButton />
      </BNavbarNav>
    </BNavbar>

    <AddServerFormDialog
      v-if="!isSelfHosted"
      :show="showAddServerDialog"
      :existing-server-names="allServerNames"
      @close="showAddServerDialog = false"
      @add-server="addServer($event)"
    />

    <div v-if="apiInfo !== void 0">
      <div
        class="split left"
        :style="{ width: showClientMethods ? '80%' : '100%' }"
      >
        <div class="apiInfo">
          <ApiInfo :info="apiInfo.info" />
          <div v-for="(hub, index) in apiInfo.hubs" :key="hub.path">
            <ApiHub
              :color="colorsService.getColor(index)"
              :server-info="selectedServerInfo"
              :hub="hub"
            />
          </div>
        </div>
        <ClientMethodPanelButton
          class="clientmethods-button"
          :style="{ left: showClientMethods ? '80%' : '100%' }"
          :number-of-client-methods="
            $root.$data.ClientMethodsService.clientMethods.length
          "
          :checked="false"
          @click="showClientMethods = !showClientMethods"
        />
      </div>
      <div
        class="split right"
        :style="{ width: showClientMethods ? '20%' : '0%' }"
      >
        <ClientMethodsPanel
          :client-methods="$root.$data.ClientMethodsService.clientMethods"
        />
      </div>
    </div>
    <div v-else>
      <BAlert
        v-if="apiInfoErrorMessage !== void 0"
        show
        variant="danger"
        class="error"
      >
        {{ apiInfoErrorMessage }}
      </BAlert>
    </div>
  </div>
</template>

<script>
import ApiInfo from "./components/ApiInfo.vue";
import ApiHub from "./components/ApiHub.vue";
import {
  BAlert,
  BButton,
  BFormSelect,
  BInputGroup,
  BInputGroupAppend,
  BNavbar,
  BNavbarNav,
  BNavbarBrand,
} from "bootstrap-vue";
import ClientMethodsPanel from "./components/ClientMethodsPanel.vue";
import ClientMethodPanelButton from "./components/ClientMethodPanelButton.vue";
import SearchBox from "./components/SearchBox.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { ColorsService } from "./services/Colors.service";
import AddServerFormDialog from "./components/AddServerFormDialog";
import { Validator } from "jsonschema";
import ThemeToggleButton from "./components/ThemeToggleButton";

export default {
  name: "SignalRUi",
  components: {
    AddServerFormDialog,
    ApiInfo,
    ApiHub,
    BAlert,
    BButton,
    BFormSelect,
    BInputGroup,
    BInputGroupAppend,
    BNavbar,
    BNavbarBrand,
    BNavbarNav,
    ClientMethodsPanel,
    ClientMethodPanelButton,
    SearchBox,
    ThemeToggleButton,
  },
  data: function () {
    return {
      isSelfHosted: false,
      apiInfoErrorMessage: void 0,
      apiInfo: {
        info: {},
        hubs: [],
        definitions: {},
      },
      selectedServer: null,
      servers: [
        {
          name: String,
          url: String,
          ws: String,
          docs: String,
          origin: {
            type: "config" | "local",
            validator: function (value) {
              return ["config", "local"].indexOf(value) !== -1;
            },
          },
        },
      ],
      searchString: "",
      showClientMethods: false,
      showAddServerDialog: false,
      colorsService: new ColorsService(),
    };
  },
  computed: {
    selectServerOptions: function () {
      if (!this.servers) {
        return [];
      }

      return this.servers.map((s) => {
        return {
          value: s.name,
          text: `${s.name} (${s.url})${s.origin === "local" ? " *" : ""}`,
        };
      });
    },
    selectedServerInfo: function () {
      if (!this.selectedServer) {
        return void 0;
      }

      const selectedServerInfo = this.servers.filter(
        (x) => x.name === this.selectedServer
      );
      if (selectedServerInfo.length != 1) {
        console.warn(
          `None or more than 1 server with name ${this.selectedServer} were found. The server name must be unique.`
        );
        return void 0;
      }

      return selectedServerInfo[0];
    },
    allServerNames: function () {
      return this.servers ? this.servers.map((s) => s.name) : [];
    },
  },
  async mounted() {
    this.apiInfo = void 0;

    const configFile = "./config.json";
    const errorMessage = `Failed to load configuration file from ${configFile}.`;
    try {
      const config = await this.getJson(configFile);
      this.servers = config.servers.map((s) => {
        return { ...s, origin: "config" };
      });

      const localServers = this.getLocalServers();
      if (localServers !== void 0) {
        this.servers.push(...localServers);
      }

      const lastSelectedServer = localStorage.selectedServer;
      if (this.servers.length > 0) {
        this.selectedServer =
          lastSelectedServer === void 0
            ? this.selectServerOptions[0].value
            : lastSelectedServer;
        this.selectServer();
      }
    } catch (e) {
      // eslint-disable-next-line
      console.warn(`${errorMessage + " " + e.message}. Falling back to self-hosted at localhost.`)
      this.setupSelfHosted(); // falls back to self-hosted in case config cannot be loaded or isn't found
    }
  },
  methods: {
    getJson(url) {
      return new Promise(function (resolve, reject) {
        const xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = () => {
          if (xhr.status >= 200 && xhr.status < 300) {
            try {
              const json = JSON.parse(xhr.response);
              resolve(json);
            } catch (e) {
              reject(e.message);
            }
          } else {
            reject(xhr.statusText);
          }
        };
        xhr.onerror = () => reject(xhr.statusText);
        xhr.send();
      });
    },
    setupSelfHosted() {
      this.isSelfHosted = true;
      let path = window.location.pathname;
      if (path) {
        if (path.startsWith("/")) {
          path = path.slice(1);
        }
        if (path && !path.endsWith("/")) {
          path = `${path}/`;
        }
      }
      const localhostInfo = {
        name: window.location.hostname,
        url: `${window.location.protocol}//${window.location.host}`,
        ws: `ws://${window.location.host}`,
        docs: `${path}api.json`,
      };
      this.servers = [localhostInfo];
      this.selectServerByName(localhostInfo.name);
    },
    selectServer: async function () {
      this.apiInfo = void 0;
      this.apiInfoErrorMessage = void 0;
      this.$root.$data.ClientMethodsService.clearAll();

      if (!this.isSelfHosted) {
        // only save selected server if not self-hosted, otherwise the server is always the same
        if (this.selectedServerInfo !== void 0) {
          localStorage.selectedServer = this.selectedServerInfo.name;
        } else {
          localStorage.removeItem("selectedServer");
          return;
        }
      }

      const docUrl = `${this.selectedServerInfo.url}/${this.selectedServerInfo.docs}`;
      try {
        const apiDoc = await this.getJson(docUrl);
        const hubApiSchema = await this.getJson("./hubApiSchema.json");
        let jsonSchemaValidator = new Validator();
        let validationResult = jsonSchemaValidator.validate(
          apiDoc,
          hubApiSchema
        );
        if (validationResult.errors.length > 0) {
          throw new Error(validationResult.toString());
        }

        this.apiInfo = apiDoc;
        this.$root.$data.typeDefinitionsService = new TypeDefinitionsService(
          this.apiInfo
        );
      } catch (e) {
        this.apiInfoErrorMessage = `:~( could not retrieve api documentation from ${docUrl}. ${
          e.message != void 0 ? e.message : ""
        }`;
      }
    },
    addServer(server) {
      server.origin = "local";
      this.servers.push(server);
      this.saveLocalServers();
      this.selectServerByName(server.name);
    },
    removeSelectedServer() {
      let serverToRemove = this.selectedServer;
      for (let i = 0; i < this.servers.length; ++i) {
        if (this.servers[i].name === serverToRemove) {
          if (this.servers.length > 1) {
            if (i === 0) {
              this.selectedServer = this.servers[1].name;
            }
            this.selectedServer = this.servers[i - 1].name;
          } else {
            this.selectedServer = void 0;
          }
          this.selectServer();
          this.servers.splice(i, 1);
          this.saveLocalServers();
          break;
        }
      }
    },
    selectServerByName(serverName) {
      this.selectedServer = serverName;
      this.selectServer();
    },
    getLocalServers() {
      if (localStorage.getItem("localServers")) {
        try {
          return JSON.parse(localStorage.getItem("localServers"));
        } catch (e) {
          localStorage.removeItem("localServers");
        }
      }
    },
    saveLocalServers() {
      const localServers = this.servers.filter((s) => s.origin === "local");
      if (localServers.length > 0) {
        const localServersStr = JSON.stringify(
          this.servers.filter((s) => s.origin === "local")
        );
        localStorage.setItem("localServers", localServersStr);
      } else {
        localStorage.removeItem("localServers");
      }
    },
  },
};
</script>

<style lang="scss">
#SignalRUi {
  font-family: $font-family;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;

  .input-group-text {
    color: var(--primary-text);
    background-color: var(--navbar-background);
    border-color: var(--primary-border);
  }

  .custom-select {
    color: var(--primary-text);
    background-color: var(--background);
    border-color: var(--primary-border);
  }

  .logo {
    height: 32px;
    opacity: 0.9;
  }

  .split {
    position: absolute;
    top: 45px;
    bottom: 0;
    z-index: 1;
    overflow: auto;
  }

  .left {
    background: var(--background);
    width: 80%;
    left: 0;
    transition: width 0.2s;
  }

  .right {
    background: var(--background);
    width: 20%;
    border-left: 1px solid var(--primary-border);
    right: 0;
    transition: width 0.2s;
  }

  .apiInfo {
    margin: 20px;
  }

  .error {
    margin: 20px;
    font-size: 16px;
  }

  .navBar {
    background: var(--navbar-background);
    border-bottom: 1px solid var(--primary-border);
    height: 45px;

    .navBarTitle {
      font-size: 20px;
      color: var(--primary-text);
    }
  }

  .clientmethods-button {
    float: right;
    position: fixed;
    border-radius: 50%;
    top: 50px;
    left: 100%;
    margin-left: -65px;
    transition: left 0.2s;
  }
}
</style>
