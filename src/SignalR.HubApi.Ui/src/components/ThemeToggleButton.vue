<template>
  <button
    :title="theme == 'dark' ? 'Dark Theme' : 'Light Theme'"
    @click="toggleTheme()"
    class="theme-toggle-button"
  >
    <b-icon
      v-if="theme === 'dark'"
      class="theme-toggle-button-icon"
      icon="moon"
      font-scale="1.5"
      aria-hidden="true"
    />
    <b-icon
      v-else
      class="theme-toggle-button-icon"
      icon="brightness-high"
      font-scale="1.5"
      aria-hidden="true"
    />
  </button>
</template>

<script>
import { BIcon } from "bootstrap-vue";

export default {
  name: "ThemeToggleButton",
  components: {
    BIcon,
  },
  data: function () {
    return {
      theme: localStorage.getItem("theme"),
    };
  },
  mounted() {
    if (!this.theme || this.theme === "light") {
      this.setTheme("light");
    } else {
      this.setTheme("dark");
    }
  },
  methods: {
    toggleTheme() {
      if (this.theme === "light") {
        this.setTheme("dark");
      } else {
        this.setTheme("light");
      }
    },
    setTheme(theme) {
      this.theme = theme;
      document.documentElement.setAttribute("theme", theme);
      localStorage.setItem("theme", theme);
    },
  },
};
</script>

<style lang="scss" scoped>
button:focus {
  outline: 0;
}

.theme-toggle-button {
  border: none;
  background: var(--navbar-background);
  padding-left: 10px;
}

.theme-toggle-button-icon {
  color: var(--primary);
}
</style>
