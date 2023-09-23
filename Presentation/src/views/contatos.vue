<template>
  <div @click="handleFocusOut($event)">
    <Loading v-if="loading" />
    <Header
      titulo="Contatos"
      homeModulo="/modulos"
      tooltipModulo="Home"
    />
    <div class="pa-6 app-container">
      <Grid 
        class="grid-component"
        ref="gridComponent"
        :fields="fields"
        :list="list"
        :filters="filters"
        :defaultInsertObject="defaultInsertObject"
        gridType="responsive"
        filterType="search"
        gridOverflow="vertical"
        :gridResizable="true"
        @listarItens="listarItens($event)"
        @selecionarItem="selecionarItem($event)"
        @exportarExcel="exportarExcel($event)"
        @salvarItem="salvarItem()"
        @botaoClick="botaoClick($event)"
        :minimizedItemList="[]"
        :hideExport="true"
        :hideInsert="true"
        :hideEdit="true"
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
      <MensagemErro
        :mensagemAlerta="mensagemAlerta"
        :alertaErro="alertaErro"
        @ocultarErro="alertaErro = false"
        :login="false"
      />
      <v-row justify="center" v-if="item">
        <v-dialog v-model="modalDadosMestre" max-width="500px" @click:outside="handleFocusOut($event)">
          <v-card>
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>Usuário</v-toolbar-title>
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
                      label="Nome"
                      dense
                      outlined
                      rounded
                      v-model="item.nome"
                      readonly
                      height="60px"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Login"
                      dense
                      v-model="item.login"
                      outlined
                      rounded
                      ref="refLoginDuplicado"
                      readonly
                      height="60px"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Login"
                      dense
                      v-model="item.perfilDescricao"
                      outlined
                      rounded
                      ref="refLoginDuplicado"
                      readonly
                      height="60px"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="E-mail"
                      dense
                      v-model="item.email"
                      outlined
                      rounded
                      readonly
                      height="60px"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Status"
                      dense
                      v-model="item.statusFormatado"
                      outlined
                      rounded
                      readonly
                      height="60px"
                    />
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
          </v-card>
        </v-dialog>
      </v-row>
    </div>
  </div>
</template>

<script>
import Loading from "../components/loading.vue";
import Header from "../components/header.vue";
import Paginacao from "../components/paginacao.vue";
import Grid from "../components/grid.vue";
import UsuarioService from "../service/usuario-service.js";
import ModalSucesso from "../components/modal-sucesso.vue";
import MensagemErro from "../components/mensagem-erro.vue";

const usuarioService = new UsuarioService();

export default {
  components: {
    Loading,
    Header,
    Paginacao,
    Grid,
    ModalSucesso,
    MensagemErro,
  },
  data() {
    return {
      mensagemTitulo: "",
      mensagemAlerta: "",
      alertaSucesso: false,
      alertaErro: false,
      loading: false,
      filters: usuarioService.listarFiltrosContatos(),
      modalDadosMestre: false,
      item: null,
      perfis: [],
      setores: [],
      setoresIds: null,
      labelSetorCadastrado: "",
      validado: false,
      novaSenha: "",
      novaSenhaConfirmacao: "",
      showNovaSenha: false,
      showConfirmarSenha: false,
      modalSenha: false,
      mensagemAlteracao: "",
      alertConfirmacao: false,
      totalPages: 1,
      totalItems: 1,
      fields: [],
      list: [],
      defaultInsertObject: {
        id: null,
        nome: "",
        email: "",
        login: "",
        ativo: true,
        restringeSetor :true,
        senha: "",
        token: "",
        contatos: [],
        setoresIds: [],
        primeiroAcesso: true,
        ultimoAcesso: new Date(
          Date.now() - new Date().getTimezoneOffset() * 60000
        ).toISOString(),
        dataUltimoToken: new Date(
          Date.now() - new Date().getTimezoneOffset() * 60000
        ).toISOString(),
        perfilId: null,
        statusFormatado: "Ativo",
        alterado: false,
        selecionado: true,
        editando: true,
        nomeDuplicado: null,
        loginDuplicado: null,
      },
      viewMettaUsuarios: []
    };
  },
  methods: {
    resetarValidacao() {
      this.$refs.formDadosMestre.resetValidation();
    },
    async alterarSenha() {
      if (this.$refs.formularioAlteracao.validate()) {
        if (this.novaSenha != this.novaSenhaConfirmacao) {
          this.alertConfirmacao = true;
          this.mensagemAlteracao = "As senhas informadas não coincidem";
          setTimeout(() => {
            this.alertConfirmacao = false;
          }, 2000);
        } else {
          this.loading = true;

          let result = await usuarioService.alterarSenhaUsuario(this.item.id, this.novaSenha);
          
          if (result?.statusCode === 200) {
            this.mensagemAlerta =
              "A senha do usuário '" + this.item.nome + "' foi alterada com sucesso";
            this.mensagemTitulo = "Alteração de senha";
            this.alertaSucesso = true;
            this.limparAlterarSenha();
            this.loading = false;
          } else {
            this.mensagemAlerta = result?.data.mensagem;
            this.alertaErro = true;
            this.loading = false;
          }
        }
      }
    },
    limparAlterarSenha() {
      this.$refs.formularioAlteracao.resetValidation();
      this.mensagemAlteracao = "";
      this.alertConfirmacao = false;
      this.novaSenha = "";
      this.novaSenhaConfirmacao = "";
      this.showNovaSenha = false;
      this.showConfirmarSenha = false;
      this.modalSenha = false;
      this.item = null;
      const appContainer = document.querySelectorAll(".app-container")[0];
      appContainer.click();
    },
    exibirAlterarSenha(item) {
      this.modalSenha = true;
      this.item = item;
      this.item.primeiroAcesso = false;
      this.item.grupos = item.grupos;
    },
    handleFocusOut(event){
      this.$refs.gridComponent?.handleFocusOut(event);
    },
    selecionarItem(item){
      this.item = JSON.parse(JSON.stringify(item));
      this.setoresIds = item.setoresIds;
      this.preencherPerfis();
      this.labelSetoresCadastrados(item);
      this.modalDadosMestre = true;
    },
    async salvarItem() {
      this.validado = true;
      this.item.nomeDuplicado = null;
      this.item.loginDuplicado = null;
      if (this.$refs.formDadosMestre.validate()) {
        this.loading = true;

        if (this.item.id === null) {
          this.item.senha = this.item.login;
        }

        let result = this.item.id
          ? await usuarioService.atualizar(this.item)
          : await usuarioService.cadastrar(this.item);
        if (result?.statusCode === 200) {
          if (this.item.id) {
            this.mensagemAlerta =
              "O usuário '" + this.item.nome + "' foi editado com sucesso";
            this.mensagemTitulo = "Edição de usuário";
          } else {
            this.mensagemAlerta =
              "O usuário '" + this.item.nome + "' foi inserido com sucesso";
            this.mensagemTitulo = "Inserção de usuário";
          }
          this.alertaSucesso = true;
          this.closeModalDadosMestre();
          this.$refs.formDadosMestre.resetValidation();
          await this.listarItens();
        } else if (result?.statusCode === 409) {
          if (result?.data.data.campoErro == "nome") {
            this.item.nomeDuplicado = result?.data.mensagem;
            this.$refs.refNomeDuplicado.focus();
          } else {
            this.item.loginDuplicado = result?.data.mensagem;
            this.$refs.refLoginDuplicado.focus();
          }
          this.loading = false;
        } else {
          this.mensagemAlerta = result?.data.mensagem;
          this.alertaErro = true;
          this.loading = false;
        }
      }
    },
    selecionarNome(nome){
      if(nome){
        nome = nome.colaborador ? nome.colaborador : nome; 
        this.item.nome = nome;
        const colaboradores = this.viewMettaUsuarios.filter(x => {
          return x.colaborador == nome;
        });
        if(colaboradores.length > 0)
          this.item.email = colaboradores[0].email;
      }
    },
    adicionarContato(item){
      item.contatos.push(null);
    },
    excluirContato(item, index){
      item.contatos.splice(index, 1);
    },
    async getDescricoesPerfil() {
      this.fields = await usuarioService.listarCamposContatos();
    },
    alterarPagina(pagina) {
      this.filters.pagina = pagina;
      this.listarItens();
    },
    alterarItensPorPagina(itens) {
      this.filters.itensPagina = itens;
      this.filters.pagina = 1;
      this.listarItens();
    },
    async listarItens() {
      this.loading = true;
      let result = await usuarioService.listarUsuariosContatos(this.filters);
      if (result?.statusCode === 200) {
        this.list = result?.data.lista;
        this.totalPages = result?.data.paginas;
        this.totalItems = result?.data.totalItens;
        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    excluirSetores(item) {
      if(this.setoresIds != null) {
        item.setoresIds = [];
        this.labelSetoresCadastrados(item);
      } else {
        item.setoresIds = null;
        this.labelSetoresCadastrados(item);
      }
    },
    labelSetoresCadastrados(item) {
      if(item.setoresIds == null) {
        this.labelSetorCadastrado = "Nenhum setor cadastrado";
      } else if(item.setoresIds.length == 0) {
        this.labelSetorCadastrado = "Excluir Setores";
      } else {
        this.labelSetorCadastrado = "";
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
    ocultarSucesso() {
      this.alertaSucesso = false;
      this.loading = false;
      this.listarItens();
    },
    preencherPerfis() {
      if (!this.perfis || this.perfis.length == 0) {
        this.perfis = this.fields.filter((x) => {
          return x.valor == "perfilDescricao";
        })[0]?.lista;
      }
    },
    botaoClick(item){
      this.item = item;
      this.modalSenha = true;
    }
  },
  created: function () {
    this.getDescricoesPerfil();
    this.listarItens();
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
</style>