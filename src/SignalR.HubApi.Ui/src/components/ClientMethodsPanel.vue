<template>
  <div id="ClientMethodsPanel">
    <div class="panel">
      <ClientMethodCard
        v-for="(note, index) in clientMethods"
        :key="`note-${note.timestampStr()}-${index}`"
        :client-method="note"
      />
    </div>
  </div>
</template>

<script>
import ClientMethodCard from "./ClientMethodCard.vue";

export default {
  name: "ClientMethodsPanel",
  components: {
    ClientMethodCard,
  },
  props: {
    clientMethods: Array,
  },
};
</script>

<style lang="scss" scoped>
#ClientMethodsPanel {
  .panel {
    margin-left: 4px;
    margin-right: 4px;
  }
}
</style>
