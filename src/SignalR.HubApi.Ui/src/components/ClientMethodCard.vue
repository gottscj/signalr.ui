<template>
  <div id="ClientMethodCard">
    <div class="clientmethod-progress" v-bind:style="{ opacity: opacity }">
      <div class="clientmethod-card" v-on:click="toggleShowClientMethod">
        <div
          class="clientmethod-card-color"
          v-bind:style="{ background: clientMethod.color }"
        />
        <div class="clientmethod-card-service-title">
          {{ clientMethod.hubName }}
          <div class="clientmethod-card-title">
            {{ clientMethod.title }}
          </div>
        </div>
        <div class="clientmethod-timestamp">
          {{ clientMethod.timestampStr() }}
        </div>
      </div>
      <div class="progress">
        <div class="progressbar" v-bind:style="{ width: `${progress}%` }" />
      </div>
    </div>
    <ModalDialog v-if="showClientMethod" v-on:close="toggleShowClientMethod">
      <div class="clientmethod-fullcard-title">
        <div
          class="clientmethod-fullcard-color"
          v-bind:style="{ background: clientMethod.color }"
        />
        <div class="clientmethod-card-service-title">
          {{ clientMethod.hubName }}
          <div class="clientmethod-card-title">
            {{ clientMethod.title }}
          </div>
        </div>
        <div class="clientmethod-timestamp">
          {{ clientMethod.timestampStr() }}
        </div>
      </div>
      <div class="clientmethod-fullcard-content">
        <pre>{{ clientMethod.content }}</pre>
      </div>
    </ModalDialog>
  </div>
</template>

<script>
import { ClientMethod } from "../models/ClientMethod.model";
import ModalDialog from "./ModalDialog.vue";

export default {
  name: "ClientMethodCard",
  components: {
    ModalDialog
  },
  props: {
    clientMethod: ClientMethod
  },
  data: function() {
    return {
      showClientMethod: false,
      updateTimer: void 0,
      opacity: 1,
      progress: 100
    };
  },
  watch: {
    clientMethod: function() {
      this.opacity = this.calculateOpacity();
      this.progress = this.calculateProgress();
    }
  },
  mounted() {
    const self = this;
    this.opacity = this.calculateOpacity();
    this.progress = this.calculateProgress();
    this.updateTimer = setInterval(() => {
      self.opacity = self.calculateOpacity();
      self.progress = self.calculateProgress();
    }, 2000);
  },
  beforeDestroy() {
    clearInterval(this.updateTimer);
  },
  methods: {
    toggleShowClientMethod() {
      this.showClientMethod = !this.showClientMethod;
    },
    calculateOpacity: function() {
      return 1 - 0.8 * (this.clientMethod.getElapsed() / this.cardTimeout);
    },
    calculateProgress: function() {
      return 100 - 100 * (this.clientMethod.getElapsed() / this.cardTimeout);
    }
  },
  computed: {
    cardTimeout: function() {
      return this.$root.$data.ClientMethodsService.clientMethodTimeoutMs;
    }
  }
};
</script>

<style lang="scss" scoped>
#ClientMethodCard {
  min-width: 300px;

  .clientmethod-progress {
    margin-top: 4px;
    margin-bottom: 6px;
  }

  .clientmethod-card-color {
    float: left;
    height: 38px;
    width: 10px;
    border-radius: 4px 0px 0px 4px;
  }

  .clientmethod-card-service-title {
    margin-top: 2px;
    margin-left: 10px;
    font-size: 10px;
  }

  .clientmethod-card-title {
    font-size: 14px;
  }

  .clientmethod-card {
    display: flex;
    height: 40px;
    background-color: map-get($accent-color, 30);
    color: inherit;
    cursor: pointer;
    width: 100%;
    text-align: left;
    outline: none;
    font-size: 12px;
    border-style: solid;
    border-width: 1px;
    border-radius: 5px 5px 5px 5px;
    border-color: map-get($accent-color, 400);
    transition: opacity 2s;
  }

  .clientmethod-fullcard-color {
    float: left;
    height: 38px;
    width: 10px;
    border-radius: 4px 0px 0px 0px;
  }

  .clientmethod-fullcard-title {
    @extend .clientmethod-card;
    cursor: default;
    background-color: map-get($accent-color, 400);
    font-size: 14px;
    border-radius: 5px 5px 0px 0px;
    margin-top: 0px;
    margin-bottom: 0px;
  }

  .clientmethod-fullcard-content {
    @extend .clientmethod-card;
    display: block;
    cursor: default;
    border-radius: 0px 0px 5px 5px;
    margin-top: 0px;
    margin-bottom: 0px;
    padding: 5px;
    border-top: 0px;
    max-height: 500px;
    height: auto;
    overflow: auto;
  }

  .clientmethod-fullcard-content pre {
    margin: 0px;
    font-size: 14px;
    color: map-get($primary-color, 900);
  }

  .clientmethod-timestamp {
    margin-top: 10px;
    margin-right: 10px;
    margin-left: auto;
    //float: right;
    height: 20px;
    background-color: map-get($accent-color, 100);
    color: map-get($primary-color, 900);
    padding-left: 4px;
    padding-right: 4px;
    border-radius: 3px 3px 3px 3px;
  }

  .progress {
    margin: -1px 3px 0px 3px;
    height: 1px;
    background-color: map-get($accent-color, 30);
  }

  .progressbar {
    width: 100%;
    background-color: map-get($accent-color, 400);
    transition: width 5s;
  }
}
</style>
