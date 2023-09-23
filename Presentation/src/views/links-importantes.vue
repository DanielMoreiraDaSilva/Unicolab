<template>
  <div>
    <Header titulo="Links Importantes"  homeModulo="/modulos" tooltipModulo="Home"/>
    <div class="pa-6 mb-0 mt-16 app-container">
       <v-row class="mt-6 pl-0">
        <v-col
          cols="12"
          sm="12"
          md="6"
          lg="4"
          v-for="(link, index) in links"
          :key="link.id"
          style="display: flex; justify-content: center;"
        >
          <CardModulo
            :novaAba="true"
            v-bind:card="link"
            v-bind:class="index == links.length - 1 ? 'ultimoItem' : ''"
          ></CardModulo>
        </v-col>
      </v-row>
    </div>
    <ModalErro
      :mensagem="mensagemAlerta"
      :alertaErro="alertaErro"
      @ocultarErro="ocultarErro"
    />
  </div>
</template>
<script>
import Header from "../components/header.vue";
import CardModulo from "../components/card-selecao.vue";
import LinkImportanteService from "../service/links-importantes-service.js"
import ModalErro from "../components/mensagem-erro.vue";

const linkImportanteService = new LinkImportanteService();

export default {
  components: {
    Header,
    CardModulo,
    ModalErro
  },
  data() {
    return {
      paginas: [],
      links: [],
      mensagemAlerta: null,
      alertaErro: false,
    };
  },
  methods: {
    ocultarErro(){
      this.mensagemAlerta = null;
      this.alertaErro = false;
    },
    async listarLinks(){
      this.loading = true;
      let result = await linkImportanteService.listarLinksImportantes();
      if (result?.statusCode === 200) {
          this.links = result?.data;
          this.loading = false;
      } else {
        this.loading = false;
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
      }
    }
  },
  created: function() {
    this.listarLinks();
  },
};
</script>