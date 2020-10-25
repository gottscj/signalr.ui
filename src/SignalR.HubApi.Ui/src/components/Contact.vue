<template>
  <address id="Contact">
    Contact:
    <a
      v-if="!!email && !!name"
      class="contact-element"
      :href="`mailto:${email}`"
      >{{ name }}</a
    >
    <template v-if="!!email && !!name && !!url" class="contact-element">
      -
    </template>
    <a v-if="!!url" class="contact-element" :href="url">{{ url }}</a>
  </address>
</template>

<script>
export default {
  name: "Contact",
  props: {
    name: String,
    email: String,
    url: String,
  },
};
</script>

<style lang="scss" scoped>
#Contact {
  font-size: 12px;
  color: var(--primary-text);

  .contact-element {
    color: var(--primary-text);
  }
}
</style>
