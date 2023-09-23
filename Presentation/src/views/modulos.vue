<template>
  <div>
    <Header titulo="Home" />
    <div class="pa-6 mb-0 mt-16 app-container" >
      <v-row class="mt-6 pl-0">
        <v-col
          cols="12"
          sm="12"
          md="6"
          lg="4"
          v-for="(modulo, index) in modulos"
          :key="modulo.id"
          style="display: flex; justify-content: center;"
        >
          <CardModulo
            v-bind:card="modulo"
            v-bind:class="index == modulos.length - 1 ? 'ultimoItem' : ''"
          ></CardModulo>
        </v-col>
      </v-row>
    </div>
  </div>
</template>
<script>
import Header from "../components/header.vue";
import CardModulo from "../components/card-selecao.vue";
import StorageService from '../service/storage-service.js';

const storageService = new StorageService();

export default {
  name: "Modulos",
  components: {
    Header,
    CardModulo,
  },
  data() {
    return {
      modulos: [],
    };
  },
  created: function () {
    this.modulos = JSON.parse(localStorage.getItem("login"))?.perfil.modulos;
  },
  mounted: async function () {
    await storageService.validarToken();
    setTimeout(() => {
      document.getElementsByTagName("html")[0].style.overflow = "hidden auto";
    }, 100);
  },
};
</script>