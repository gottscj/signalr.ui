<template>
  <transition name="modal">
    <div class="dialog-mask" @click="$emit('close')">
      <div class="dialog-wrapper">
        <div class="dialog-container">
          <slot />
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: "ModalDialog",
  emits: ["close"],
};
</script>

<style lang="scss" scoped>
.dialog-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.8);
  display: table;
  transition: opacity 0.2s ease;
}

.dialog-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.dialog-container {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  width: 50%;
  margin: auto;
  transition: all 0.2s ease;
}

.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

/*
 * The following styles are auto-applied to elements with
 * transition="modal" when their visibility is toggled
 * by Vue.js.
 */

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
