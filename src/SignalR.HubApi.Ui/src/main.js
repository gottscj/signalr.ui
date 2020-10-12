import Vue from "vue";
import SignalRUi from "./SignalRUi.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { ClientMethodsService } from "./services/ClientMethods.service";

Vue.config.productionTip = false;

new Vue({
  data: function () {
    return {
      typeDefinitionsService: TypeDefinitionsService,
      ClientMethodsService: new ClientMethodsService(),
    };
  },
  render: (h) => h(SignalRUi),
}).$mount("#SignalRUi");
