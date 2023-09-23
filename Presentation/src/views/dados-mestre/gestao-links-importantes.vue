<template>
  <div @click="handleFocusOut($event)">
    <Header titulo="Gestão Links Importantes"  homeModulo="/dados-mestre" tooltipModulo="Dados Mestre"/>
    <div class="pa-6 app-container">
      <Loading v-if="loading" />
      <Grid 
        class="grid-component"
        ref="gridComponent"
        :fields="fields"
        :list="links"
        :filters="filters"
        :defaultInsertObject="defaultInsertObject"
        gridType="responsive"
        filterType="search"
        gridOverflow="vertical"
        :gridResizable="true"
        @listarItens="listarLinks($event)"
        @selecionarItem="selecionarItem($event)"
        @exportarExcel="exportarExcel($event)"
        @salvarItem="salvarItem()"
        @botaoClick="botaoClick($event)"
        :minimizedItemList="[]"
        :hideExport="true"
      />
      <Paginacao
        id="paginacao"
        :totalPaginas="totalPages"
        :paginaAtual="filters.pagina"
        :totalItens="totalItems"
        @alterarItensPorPagina="alterarItensPorPagina($event)"
        @alterarPagina="alterarPagina($event)"
      />
      <ModalSucesso
        :mensagem="mensagemAlerta"
        :alertaSucesso="alertaSucesso"
        :titulo="mensagemTitulo"
        @ocultarSucesso="ocultarSucesso"
      />
      <ModalErro
        :mensagemAlerta="mensagemAlerta"
        :alertaErro="alertaErro"
        @ocultarErro="alertaErro = false"
        :login="false"
      />
      <v-row justify="center" v-if="item">
        <v-dialog content-class="rounded-xl" v-model="modalDadosMestre" max-width="500px">
          <v-card rounded="xl">
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>{{
                item.id ? "Editar Usuário" : "Inserir Usuário"
              }}</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-btn icon dark @click="closeModalDadosMestre">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </v-toolbar>
            <v-card-text>
              <v-form ref="formDadosMestre" class="pt-4">
                <v-row class="pt-2 pb-4">
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      ref="refTituloDuplicado"
                      label="Título"
                      dense
                      clearable
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      v-model="item.titulo"
                      :error-messages="item.tituloDuplicado"
                      :rules="[(v) => !!v || 'O campo título é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      ref="refLinkDuplicado"
                      label="Link"
                      dense
                      clearable
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      v-model="item.url"
                      :error-messages="item.linkDuplicado"
                      :rules="[(v) => !!v || 'O campo link é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-textarea
                      label="Descrição"
                      dense
                      clearable
                      outlined
                      rounded
                      maxlength="80"
                      counter="80"
                      auto-grow
                      no-resize
                      height="90px"
                      class="alinhamentoTextIcon"
                      v-model="item.descricao"
                      :rules="[(v) => !!v || 'O campo descrição é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0" v-if="item.id != null">
                    <v-select
                      v-model="item.ativo"
                      item-text="text"
                      item-value="value"
                      :items="[
                        { value: true, text: 'Ativo' },
                        { value: false, text: 'Inativo' },
                      ]"
                      attach
                      outlined
                      rounded
                      label="Status"
                      dense
                      height="60px"
                      class="alinhamentoIcon"
                    ></v-select>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
            <v-card-actions class="pt-0">
              <v-spacer></v-spacer>
              <v-btn
                color="red darken-1"
                text
                right
                rounded
                @click="closeModalDadosMestre"
              >
                <v-icon>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                @click="salvarItem"
              >
                <v-icon>mdi-check</v-icon>
                {{ item.id ? "Salvar" : "Inserir" }}
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
    </div>
  </div>
</template>
<script>
import Header from "../../components/header.vue";
import LinkImportanteService from "../../service/links-importantes-service.js"
import Paginacao from "../../components/paginacao.vue";
import Grid from "../../components/grid.vue";
import ModalSucesso from "../../components/modal-sucesso.vue";
import ModalErro from "../../components/mensagem-erro.vue";
import Loading from "../../components/loading.vue";

const linkImportanteService = new LinkImportanteService();

export default {
  components: {
    Header,
    ModalErro,
    ModalSucesso,
    Grid,
    Paginacao,
    Loading,
  },
  data() {
    return {
      loading: false,
      item: null,
      paginas: [],
      links: [],
      mensagemAlerta: null,
      mensagemTitulo: null,
      alertaSucesso: null,
      alertaErro: false,
      totalPages: 1,
      totalItems: 1,
      fields: [],
      filters: linkImportanteService.listarFiltros(),
      modalDadosMestre: false,
      defaultInsertObject: {
        id: null,
        titulo: "",
        descricao: "",
        url: "",
        ativo: true,
      }
    };
  },
  methods: {
    alterarPagina(pagina) {
      this.filters.pagina = pagina;
      this.listarLinks();
    },
    alterarItensPorPagina(itens) {
      this.filters.itensPagina = itens;
      this.filters.pagina = 1;
      this.listarLinks();
    },
    async salvarItem() {
      this.validado = true;
      this.item.tituloDuplicado = null;
      this.item.linkDuplicado = null;
      if (this.$refs.formDadosMestre.validate()) {
        this.loading = true;

        if (this.item.id === null) {
          this.item.senha = this.item.login;
        }

        let result = this.item.id
          ? await linkImportanteService.atualizar(this.item)
          : await linkImportanteService.cadastrar(this.item);
        if (result?.statusCode === 200) {
          if (this.item.id) {
            this.mensagemAlerta =
              "O link importante '" + this.item.titulo + "' foi editado com sucesso";
            this.mensagemTitulo = "Edição de link importante";
          } else {
            this.mensagemAlerta =
              "O link importante '" + this.item.titulo + "' foi inserido com sucesso";
            this.mensagemTitulo = "Inserção de link importante";
          }
          this.alertaSucesso = true;
          this.closeModalDadosMestre();
          this.$refs.formDadosMestre.resetValidation();
          await this.listarLinks();
        } else if (result?.statusCode === 409) {
          if (result?.data.data.campoErrado1 && result?.data.data.campoErrado2) {
            this.item.tituloDuplicado = result?.data.data.campoErrado2 + result?.data.mensagem;
            this.item.linkDuplicado = result?.data.data.campoErrado1 + result?.data.mensagem;
          } else if (result?.data.data.campoErrado == "titulo") {
            this.item.tituloDuplicado = result?.data.mensagem;
            this.$refs.refTituloDuplicado.focus();
          } else {
            this.item.linkDuplicado = result?.data.mensagem;
            this.$refs.refLinkDuplicado.focus();
          }
          this.loading = false;
        } else {
          this.mensagemAlerta = result?.data.mensagem;
          this.alertaErro = true;
          this.loading = false;
        }
      }
    },
    closeModalDadosMestre() {
      this.validado = false;
      this.loading = false;
      this.modalDadosMestre = false;
      this.item = null;
      const appContainer = document.querySelectorAll(".app-container")[0];
      appContainer.click();
    },
    selecionarItem(item){
      this.item = JSON.parse(JSON.stringify(item));
      this.modalDadosMestre = true;
    },
    handleFocusOut(event){
      this.$refs.gridComponent?.handleFocusOut(event);
    },
    ocultarErro(){
      this.mensagemAlerta = null;
      this.alertaErro = false;
    },
    ocultarSucesso() {
      this.alertaSucesso = false;
      this.loading = false;
      this.listarItens();
    },
    async listarLinks() {
      this.loading = true;
      let result = await linkImportanteService.listarGridLinksImportantes(this.filters);
      if (result?.statusCode === 200) {
        this.links = result?.data.lista;
        this.totalPages = result?.data.paginas;
        this.totalItems = result?.data.totalItens;
        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    async listarFields() {
      this.fields = await linkImportanteService.listarCampos();
    }
  },
  created: function() {
    this.listarFields();
    this.listarLinks();
  },
};
</script>

<style scoped>
.grid-component{
  max-height: calc(100vh - 200px) !important;
  font-family: 'Poppins';
  font-weight: 900;
}

.v-text-field {
  color: #3b71fe !important; 
  font-family: 'Poppins';
  font-weight: normal;
}

.v-autocomplete {
  color: #3b71fe !important; 
  font-family: 'Poppins';
  font-weight: normal;
}

.v-card {
  border-radius: 0.5em;
  font-family: 'Poppins';
}

.v-toolbar {
  border-radius: 0.5em;
  font-family: 'Poppins';
}

.v-btn {
  font-family: 'Poppins' !important;
}

.alinhamentoIcon >>> .v-input__append-inner {
  margin-top: 18px !important;
}

.alinhamentoIcon >>> .v-label {
  margin-top: 10px
}

.alinhamentoIcon >>> .v-label--active {
  transform: translateY(-28px) scale(0.75);
}

.alinhamentoTextIcon >>> .v-input__append-inner {
  margin-top: 26px !important;
}

.alinhamentoTextIcon >>> .v-label--active {
  transform: translateY(-18px) scale(0.75);
}

</style>