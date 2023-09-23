import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    themes: {
      light: {
        primary: "#12466e",
        secondary: "#304b60",
      },
      dark: {
        primary: "#12466e",
        secondary: "#304b60",
      },
    },
  },
  icons: {
    iconfont: 'mdiSvg',
  },
});
