<template>
  <BInputGroup size="sm">
    <BFormInput
      v-model="searchString"
      :placeholder="placeholder"
      @input="$emit('input', searchString)"
    />
    <BInputGroupAppend>
      <BButton>Search</BButton>
    </BInputGroupAppend>
  </BInputGroup>
</template>

<script>
import {
  BInputGroup,
  BInputGroupAppend,
  BFormInput,
  BButton,
} from "bootstrap-vue";

export default {
  name: "SearchBox",
  components: {
    BInputGroup,
    BInputGroupAppend,
    BFormInput,
    BButton,
  },
  props: {
    value: String,
    placeholder: String,
  },
  emits: ["input"],
  data: function () {
    return {
      searchString: this.value,
    };
  },
};
</script>

<style lang="scss" scoped></style>
