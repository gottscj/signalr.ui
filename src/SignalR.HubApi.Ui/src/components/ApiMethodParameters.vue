<template>
  <div id="ApiMethodParameters">
    <textarea
      v-model="parametersJson"
      :class="
        !jsonError
          ? 'method-parameters-code-ok'
          : 'method-parameters-code-error'
      "
      placeholder="json parameters"
      :rows="parametersCodeRows"
      @change="emitParametersChange"
    />
    <div v-if="!!jsonError" class="method-parameters-error">
      {{ jsonError }}
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiMethodParameters",
  props: {
    parameters: Array,
  },
  emits: ["parameters-changed"],
  data: function () {
    return {
      parametersJson: "",
      parametersCodeRows: 1,
      jsonError: null,
    };
  },
  mounted() {
    this.parametersJson = this.createParametersJsonTemplate();
    this.emitParametersChange();
  },
  methods: {
    createParametersJsonTemplate() {
      let parametersJsonArray = [];
      this.parameters.forEach((param) => {
        parametersJsonArray.push({
          [param.name]: this.$root.$data.typeDefinitionsService.createDefaultObject(
            param.schema
          ),
        });
      });

      const parametersTemplate = JSON.stringify(parametersJsonArray, null, 2);

      this.parametersCodeRows = parametersTemplate.split("\n").length;

      return parametersTemplate;
    },
    emitParametersChange() {
      this.jsonError = null;
      let paramJson;
      try {
        paramJson = JSON.parse(this.parametersJson);
      } catch (err) {
        this.jsonError = `${err.name}: ${err.message}`;
        return;
      }

      this.$emit("parameters-changed", paramJson);
    },
  },
};
</script>

<style scoped lang="scss">
#ApiMethodParameters {
  font-size: 14px;

  textarea {
    color: var(--primary-text);
    background: var(--background);
  }

  textarea:focus {
    outline: none !important;
    border: 1px solid var(--primary-text);
  }

  .method-parameters-code-ok {
    width: 800px;
    border-color: var(--method-response-box-ok-border);
  }

  .method-parameters-code-error {
    width: 800px;
    border-color: $error-color;
  }

  .method-parameters-error {
    font-size: 12px;
    color: $error-color;
  }
}
</style>
