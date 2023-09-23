<template>
  <div>
    <Header titulo="Dados Mestre"  homeModulo="/modulos" tooltipModulo="Home"/>
   <div class="pa-6 mb-0 mt-16 app-container">
       <v-row class="mt-6 pl-0">
        <v-col
          cols="12"
          sm="12"
          md="6"
          lg="4"
          v-for="(dadoMestre, index) in dadosMestre"
          :key="dadoMestre.id"
          style="display: flex; justify-content: center;"
        >
          <CardModulo
            v-bind:card="dadoMestre"
            v-bind:class="index == dadosMestre.length - 1 ? 'ultimoItem' : ''"
          ></CardModulo>
        </v-col>
      </v-row>
    </div>
  </div>
</template>
<script>
import Header from "../../components/header.vue";
import CardModulo from "../../components/card-selecao.vue";

export default {
  components: {
    Header,
    CardModulo
  },
  data() {
    return {
      paginas: [],
      dadosMestre: [],
    };
  },
  created: function() {
    this.dadosMestre = [{id:'1', icone: 'mdi-account-group-outline', titulo: 'Usuarios', descricao: 'Gestão de usuários',url: '/dados-mestre/usuarios'},
    {id:'2', icone: 'mdi-link-variant', titulo: 'Links Importantes', descricao: 'Gestão de links importantes',url: '/dados-mestre/gestao-links-importantes'}]
  },
};
</script>