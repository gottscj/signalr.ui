<template>
  <BModal
    content-class="add-server-modal"
    ref="modal"
    title="Add a server to your local configuration"
    @show="resetForm"
    @hidden="close"
    @ok="handleOk"
  >
    <BForm @submit.stop.prevent="handleSubmit">
      <BFormGroup
        description="Server name to be used for display"
        label="Name"
        label-for="name-input"
      >
        <BFormInput
          id="name-input"
          v-model="$v.form.name.$model"
          :state="$v.form.name.$dirty ? !$v.form.name.$error : null"
          aria-describedby="name-input-live-feedback"
          placeholder="My server"
        />

        <BFormInvalidFeedback
          v-if="!$v.form.name.required"
          id="name-input-live-feedback"
        >
          Name is required
        </BFormInvalidFeedback>

        <BFormInvalidFeedback
          v-if="!$v.form.name.mustBeUnique"
          id="name-input-live-feedback"
        >
          This name is already in use. It must be unique
        </BFormInvalidFeedback>
      </BFormGroup>

      <BFormGroup
        description="Base server URL with port number"
        label="Server URL"
        label-for="url-input"
        invalid-feedback="Server URL is required"
      >
        <BFormInput
          id="url-input"
          v-model="$v.form.url.$model"
          :state="$v.form.url.$dirty ? !$v.form.url.$error : null"
          placeholder="http://localhost:5000"
        />
      </BFormGroup>

      <BFormGroup
        description="Relative URL to the json api documentation to be concatenated with the server URL"
        label="Docs relative URL"
        label-for="docs-input"
        invalid-feedback="Docs relative URL is required"
      >
        <BFormInput
          id="docs-input"
          v-model="$v.form.docs.$model"
          :state="$v.form.docs.$dirty ? !$v.form.docs.$error : null"
          placeholder="hubapi/chat/api.json"
        />
      </BFormGroup>
    </BForm>
  </BModal>
</template>

<script>
import {
  BForm,
  BFormInvalidFeedback,
  BModal,
  BFormGroup,
  BFormInput,
} from "bootstrap-vue";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";

function mustBeUnique(chosenElement) {
  return (
    !this.existingServerNames ||
    !chosenElement ||
    !this.existingServerNames
      .map((n) => n.trim().toLowerCase())
      .includes(chosenElement.trim().toLowerCase())
  );
}

export default {
  name: "AddServerFormDialog",
  components: {
    BModal,
    BForm,
    BFormInvalidFeedback,
    BFormGroup,
    BFormInput,
  },
  mixins: [validationMixin],
  props: {
    show: Boolean,
    existingServerNames: Array,
  },
  emits: ["add-server", "close"],
  data() {
    return {
      form: {
        name: null,
        url: null,
        docs: null,
      },
    };
  },
  validations: {
    form: {
      name: {
        required,
        mustBeUnique,
      },
      url: {
        required,
      },
      docs: {
        required,
      },
    },
  },
  watch: {
    show: function () {
      if (this.show) {
        this.$refs.modal.show();
      } else {
        this.close();
      }
    },
  },
  methods: {
    resetForm() {
      this.form.name = "";
      this.form.url = "";
      this.form.docs = "";
      this.$v.$reset();
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    handleSubmit() {
      // Exit when the form isn't valid
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        return;
      }

      this.$emit("add-server", {
        name: this.form.name,
        url: this.form.url,
        docs: this.form.docs,
      });

      // Hide the modal manually
      this.$nextTick(() => {
        this.close();
      });
    },
    close() {
      this.$refs.modal.hide();
      this.$emit("close");
      this.resetForm();
    },
  },
};
</script>

<style scoped>
::v-deep .add-server-modal {
  background: var(--background);
  color: var(--primary-text);
}

input {
  color: var(--primary-text);
  background-color: var(--background);
  border-color: var(--primary-border);
}

input:focus {
  color: var(--primary-text);
  background-color: var(--navbar-background);
  border-color: var(--primary-border);
}
</style>
