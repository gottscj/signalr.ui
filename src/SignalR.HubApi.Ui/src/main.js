import Vue from "vue";
import SignalRUi from "./SignalRUi.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { ClientMethodsService } from "./services/ClientMethods.service";
import {
  BIcon,
  BIconMoon,
  BIconBrightnessHigh,
  BIconChatRightDots,
  BIconChevronRight,
  BIconChevronDown,
} from "bootstrap-vue";

Vue.config.productionTip = false;

Vue.component("BIcon", BIcon);
Vue.component("BIconMoon", BIconMoon);
Vue.component("BIconBrightnessHigh", BIconBrightnessHigh);
Vue.component("BIconChatRightDots", BIconChatRightDots);
Vue.component("BIconChevronRight", BIconChevronRight);
Vue.component("BIconChevronDown", BIconChevronDown);

new Vue({
  data: function () {
    return {
      typeDefinitionsService: TypeDefinitionsService,
      ClientMethodsService: new ClientMethodsService(),
    };
  },
  render: (h) => h(SignalRUi),
}).$mount("#SignalRUi");
