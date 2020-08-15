<template>
  <div id="ClientMethodPanelButton">
    <BButton class="clientmethod-button" v-on:click="$emit('click')">
      <img class="clientmethod-icon" src="../assets/clientmethod.svg" />
      <BBadge class="clientmethod-count" v-if="numberOfClientMethods > 0">{{
        numberOfClientMethods
      }}</BBadge>
    </BButton>
  </div>
</template>

<script>
import { BButton, BBadge } from "bootstrap-vue";

export default {
  name: "ClientMethodPanelButton",
  components: {
    BBadge,
    BButton
  },
  props: {
    numberOfClientMethods: Number
  }
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
    height: 35px;
  }

  .clientmethod-count {
    background-color: map-get($accent-color, 200);
    color: map-get($primary-color, 900);
    border-radius: 50px;
    position: absolute;
    top: 0px;
    right: 7px;
  }
}
</style>
