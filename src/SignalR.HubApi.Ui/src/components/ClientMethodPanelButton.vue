<template>
  <div id="ClientMethodPanelButton">
    <BButton class="clientmethod-button" @click="$emit('click')">
      <b-icon
        class="clientmethod-icon"
        icon="chat-right-dots"
        font-scale="2"
        aria-hidden="true"
      />
      <BBadge v-if="numberOfClientMethods > 0" class="clientmethod-count">
        {{ numberOfClientMethods }}
      </BBadge>
    </BButton>
  </div>
</template>

<script>
import { BButton, BBadge, BIcon } from "bootstrap-vue";

export default {
  name: "ClientMethodPanelButton",
  components: {
    BBadge,
    BButton,
    BIcon,
  },
  props: {
    numberOfClientMethods: Number,
  },
  emits: ["click"],
};
</script>

<style lang="scss" scoped>
#ClientMethodPanelButton {
  .btn:focus,
  .btn:active {
    outline: none !important;
    box-shadow: none;
  }

  .clientmethod-button {
    float: left;
    position: relative;
    border-radius: 50px;
    background: none;
    border: none;
    margin-right: -15px;
  }

  .clientmethod-icon {
    color: var(--primary);
  }

  .clientmethod-count {
    background-color: var(--client-method-count-badge-background);
    color: var(--primary);
    border-radius: 50px;
    position: absolute;
    top: 0px;
    right: 7px;
  }
}
</style>
