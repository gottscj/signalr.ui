<template>
  <div id="ApiInfo">
    <div class="api-title">
      <!-- eslint-disable-next-line -->
      {{ info.title }}<small><pre class="api-version">{{ info.version }}</pre></small>
    </div>

    <div class="detailed-info">
      <div class="api-endpoint">Endpoint: {{ info.hubApiEndpoint }}</div>

      <Contact
        v-if="!!info && !!info.contact"
        :name="info.contact.name"
        :email="info.contact.email"
        :url="info.contact.url"
      />
    </div>

    <div class="api-description">
      {{ info.description }}
    </div>
  </div>
</template>

<script>
import Contact from "./Contact";

export default {
  name: "ApiInfo",
  components: {
    Contact,
  },
  props: {
    info: {
      description: String,
      version: String,
      title: String,
      contact: {
        name: String,
        email: String,
        url: String,
      },
      hubApiEndpoint: String,
    },
  },
};
</script>

<style lang="scss">
#ApiInfo {
  margin-bottom: 30px;

  .api-title {
    color: var(--primary-text);
    font-size: 36px;
    font-weight: bold;

    small {
      font-size: 10px;
      position: relative;
      top: -5px;
      display: inline-block;
      margin: 0 0 0 5px;
      padding: 2px 8px;
      vertical-align: super;
      border-radius: 57px;
      background: var(--api-version-background);

      pre {
        margin: 0;
        padding: 1px;
        color: $light-text;
        font-size: 14px;
      }
    }
  }

  .api-endpoint {
    font-size: 12px;
    font-weight: 300 !important;
    margin: 0;
    font-weight: 600;
    color: var(--primary-text);
  }

  .api-description {
    color: var(--primary-text);
    margin: 10px;
    font-size: 15px;
  }

  .detailed-info {
    margin-left: 10px;
    padding-left: 10px;
    border-left: 2px solid var(--primary-border);
  }
}
</style>
