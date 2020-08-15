import Vue from "vue";
import SignalRUi from "./SignalRUi.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { NotificationsService } from "./services/Notifications.service";

Vue.config.productionTip = false;

new Vue({
  render: h => h(SignalRUi),
  data: {
    typeDefinitionsService: TypeDefinitionsService,
    notificationsService: new NotificationsService()
  }
}).$mount("#SignalRUi");
