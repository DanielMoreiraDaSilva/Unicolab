<template>
  <div @click="handleFocusOut($event)">
    <Loading v-if="loading" />
    <Header
      titulo="Usuários"
      homeModulo="/dados-mestre"
      tooltipModulo="Dados mestre"
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
        <v-dialog content-class="rounded-xl" v-model="modalDadosMestre" max-width="500px">
          <v-card
            rounded="xl"
          >
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
                      label="Nome"
                      dense
                      clearable
                      outlined
                      rounded
                      v-model="item.nome"
                      height="60px"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo nome é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Login"
                      dense
                      v-model="item.login"
                      clearable
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      :error-messages="item.loginDuplicado"
                      ref="refLoginDuplicado"
                      :rules="[(v) => !!v || 'O campo login é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-autocomplete
                      v-model="item.perfilId"
                      :items="perfis"
                      item-text="descricao"
                      item-value="id"
                      label="Perfil"
                      no-data-text="Nenhum perfil encontrado"
                      dense
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo perfil é obrigatório']"
                    ></v-autocomplete>
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0" >
                    <v-autocomplete
                      v-model="item.listaCursoId"
                      :items="cursos"
                      item-text="descricao"
                      item-value="id"
                      label="Curso"
                      no-data-text="Nenhum curso encontrado"
                      dense
                      outlined
                      rounded
                      multiple
                      chips
                      deletable-chips
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo curso é obrigatório']"
                    ></v-autocomplete>
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="E-mail"
                      dense
                      v-model="item.email"
                      clearable
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo e-mail é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
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
                      height="60px"
                      class="alinhamentoIcon"
                      label="Status"
                      dense
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
                class="botao-fechar"
                @click="closeModalDadosMestre"
              >
                <v-icon small>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                @click="salvarItem"
              >
                <v-icon small>mdi-check</v-icon>
                {{ item.id ? "Salvar" : "Inserir" }}
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
      <v-row justify="center">
        <v-dialog v-model="modalSenha" max-width="500px">
          <v-card>
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>Alterar Senha</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-btn icon dark @click="limparAlterarSenha">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </v-toolbar>
            <v-card-text>
              <v-form ref="formularioAlteracao">
                <v-container>
                  <v-row>
                    <v-col cols="12" class="pt-10 pl-0 pr-0 pb-0">
                      <v-text-field
                        id="password"
                        label="Nova senha"
                        prepend-icon="lock"
                        outlined
                        rounded
                        :type="showNovaSenha ? 'text' : 'password'"
                        v-on:keyup.enter="alterarSenha"
                        v-model="novaSenha"
                        :rules="[v => !!v || 'Digite a senha']"
                        :append-icon="showNovaSenha ? 'mdi-eye' : 'mdi-eye-off'"
                        @click:append="showNovaSenha = !showNovaSenha"
                      />
                    </v-col>
                    <v-col cols="12" class="pl-0 pr-0 pb-0">
                      <v-text-field
                        id="password2"
                        label="Confirme a nova senha"
                        prepend-icon="lock"
                        outlined
                        rounded
                        :type="showConfirmarSenha ? 'text' : 'password'"
                        v-on:keyup.enter="alterarSenha"
                        v-model="novaSenhaConfirmacao"
                        :rules="[v => !!v || 'Digite a senha']"
                        :append-icon="showConfirmarSenha ? 'mdi-eye' : 'mdi-eye-off'"
                        @click:append="showConfirmarSenha = !showConfirmarSenha"
                      />
                    </v-col>
                  </v-row>
                </v-container>
              </v-form>
            </v-card-text>
            <div class="pr-8 pl-8 text-center">
              <v-alert :value="alertConfirmacao" color="red" transition="scale-transition" text>
                <v-icon color="red" left>mdi-alert</v-icon>
                {{mensagemAlteracao}}
              </v-alert>
            </div>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn rounded color="red darken-1" text right @click="limparAlterarSenha">
                <v-icon>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn rounded color="#3b71fe" text @click="alterarSenha">
                <v-icon>mdi-check</v-icon>Alterar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
    </div>
  </div>
</template>

<script>
import Loading from "../../components/loading.vue";
import Header from "../../components/header.vue";
import Paginacao from "../../components/paginacao.vue";
import Grid from "../../components/grid.vue";
import UsuarioService from "../../service/usuario-service.js";
import ModalSucesso from "../../components/modal-sucesso.vue";
import MensagemErro from "../../components/mensagem-erro.vue";
import CursoService from "../../service/curso-service.js";

const usuarioService = new UsuarioService();
const cursoService = new CursoService();

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
      filters: usuarioService.listarFiltros(),
      modalDadosMestre: false,
      item: null,
      perfis: [],
      cursos: [],
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
    async listarCursos() {
      this.loading = true;
      const login = JSON.parse(localStorage.getItem("login"));
      let result = await cursoService.listarCursos(login.id);
      if (result?.statusCode === 200) {
        this.cursos = result?.data;
        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
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
    async selecionarItem(item){
      this.item = JSON.parse(JSON.stringify(item));

      if(item.id) {
        const result = await usuarioService.getById(item.id) ;
        this.item = result?.data;
        this.setoresIds = item.setoresIds;
      }

      this.preencherPerfis();
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
    async getDescricoesPerfil() {
      this.fields = await usuarioService.listarCampos();
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
      this.filters.ordenarPor = this.filters.ordenarPor - 1;
      let result = await usuarioService.listarUsuarios(this.filters);
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
    this.listarCursos();
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
  font-weight: 600;
  border: #3b71fe solid 2px !important;
  caret-color: #3b71fe;
}
.button-dark {
  background-color: #3b71fe !important;
  color: white !important;
}
.botao-fechar {
  font-family: 'Poppins' !important;
  font-weight: 600;
  border: #ff5252 solid 2px !important;
  caret-color: #ff5252;
}

.alinhamentoIcon >>> .v-input__append-inner {
  margin-top: 18px !important;
  min-height: 40px;
}

.alinhamentoIcon >>> .v-label {
  margin-top: 10px
}

.alinhamentoIcon >>> .v-label--active {
  transform: translateY(-28px) scale(0.75);
}
</style>