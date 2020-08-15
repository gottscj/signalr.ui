import Vue from "vue";
import SignalRUi from "./SignalRUi.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { ClientMethodsService } from "./services/ClientMethods.service";

Vue.config.productionTip = false;

new Vue({
  render: h => h(SignalRUi),
  data: {
    typeDefinitionsService: TypeDefinitionsService,
    ClientMethodsService: new ClientMethodsService()
  }
}).$mount("#SignalRUi");
