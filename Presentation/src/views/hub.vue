<template>
  <div>
    <Header titulo="Teste Hub" homeModulo="/modulos" tooltipModulo="Módulos" />
    <div class="pa-6 app-container">
      <v-btn @click="testarHub()">Testar Hub</v-btn>
      <div v-for="(exemplo, index) in exemplos" :key="index">
        {{exemplo.id + " - " + exemplo.descricao}}
      </div>
      <hr v-if="exemplos.length > 0">
      <div v-if="retornoGeral">Retorno Geral: {{retornoGeral}}</div>
      <div v-if="retornoEspecifico">Retorno Específico: {{retornoEspecifico}}</div>
    </div>
  </div>
</template>
<script>
import Header from "../components/header.vue";
import StorageService from "../service/storage-service.js";
import Vue from "vue";

const vue = new Vue();
const signalR = require("@microsoft/signalr");
const connection = new signalR.HubConnectionBuilder()
  .withUrl(vue.$globals.exemploHub, {
    withCredentials: false,
  })
  .build();

const storage = new StorageService();

export default {
  components: {
    Header,
  },
  data() {
    return {
      exemplos: [],
      retornoGeral: null,
      retornoEspecifico: null
    };
  },
  methods: {
    testarHub() {
      connection
        .invoke("ConsultarExemplos")
        .then((result) => {
          this.exemplos = result;
        })
        .catch(function (err) {
          return console.error(err.toString());
        });
    },
  },
  mounted: function () {
    document.getElementsByTagName("html")[0].style.overflow = "hidden auto";
  },
  beforeMount: function () {
    storage.validarToken();
  },
  created() {
    connection.start();
    connection.on("RetornoExemplo", (exemplosJson) => {
      console.log(exemplosJson);
      var retorno = JSON.parse(exemplosJson);
      if(retorno.tipo == 0)
        this.retornoGeral = retorno.mensagem;
      else
        this.retornoEspecifico = retorno.mensagem;
    });
  },
};
</script>