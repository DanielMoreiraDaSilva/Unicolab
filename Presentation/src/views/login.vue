<template>
  <div>
    <Loading v-if="loading" />
    <div class="login-bg">
      <div class="login-card-container">
        <div class="logo-background">
          <img class="logo" src="../assets/logo.png" alt="" />
        </div>
        <v-card rounded="xl" class="elevation-20 login-card">
          <div class="login-right-side">
            <h1 class="login-header">Faça seu Login</h1>
            <v-form v-model="valido" ref="formulario" class="login-form">
              <v-row>
                <v-col style="display: flex" class="pb-0">
                  <p style="margin: 2px 25px 0 60px;" class="loginESenha"> Login </p>
                  <v-text-field
                    type="text"
                    v-on:keyup.enter="entrar"
                    v-model="usuario"
                    :rules="regrasUsuario"
                    outlined
                    rounded
                    height="60px"
                    :error-messages="mensagemErroUsuario"
                    @keyup="limparErro"
                    class="loginESenha"
                    style="font-size: 20px"
                  />
                </v-col>
              </v-row>
              <v-row>
                <v-col style="display: flex" class="pt-0">
                  <p style="margin: 2px 25px 0 60px;" class="loginESenha"> Senha </p>
                  <v-text-field
                    id="password"
                    :type="'password'"
                    v-on:keyup.enter="entrar"
                    v-model="senha"
                    :rules="regrasSenha"
                    outlined
                    rounded
                    height="60px"
                    :error-messages="mensagemErroSenha"
                    @keyup="limparErro"
                    class="loginESenha"
                    style="font-size: 20px"
                  />
                </v-col>
              </v-row>
            </v-form>
            <div class="login-action">
              <div class="alerta">
                <v-alert
                  :value="alert"
                  color="red"
                  text
                  class="mr-5"
                >
                  <v-icon color="red" left>mdi-alert</v-icon>
                  {{ mensagem }}
                </v-alert>
              </div>
              <span @click="exibirEsqueceuSenha">Esqueceu sua senha?</span>
              <div class="botao">
                <v-btn
                  width="160px"
                  min-height="60px"
                  color="#3b71fe"
                  v-on:click="entrar"
                  class="mb-4"
                  dark
                  rounded
                  elevation="0"
                  >Acessar</v-btn
                >
              </div>
            </div>
          </div>
        </v-card>
        <MensagemErro
          :mensagemAlerta="mensagemAlerta"
          :alertaErro="alertaErro"
          @ocultarErro="alertaErro = false"
          :login="true"
        />
      </div>
      <v-dialog v-model="modal" max-width="600px">
        <v-card>
          <v-toolbar dark color="#3b71fe">
            <v-toolbar-title>{{ routeId ? 'Resetar senha' : 'Primeiro acesso' }}</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon dark @click="modal = false">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text>
            <v-form v-model="valido" ref="formularioAlteracao">
              <v-container>
                <v-row>
                  <v-col cols="12" class="pt-10 pl-0 pr-0 pb-0">
                    <v-text-field
                      id="password"
                      label="Nova senha"
                      prepend-icon="lock"
                      :type="showNovaSenha ? 'text' : 'password'"
                      outlined
                      rounded
                      v-on:keyup.enter="alterarSenha"
                      v-model="novaSenha"
                      :rules="regrasSenha"
                      :append-icon="showNovaSenha ? 'mdi-eye' : 'mdi-eye-off'"
                      @click:append="showNovaSenha = !showNovaSenha"
                    />
                  </v-col>
                  <v-col cols="12" class="pt-0 pl-0 pr-0 pb-0">
                    <v-text-field
                      id="password"
                      label="Confirme a nova senha"
                      prepend-icon="lock"
                      outlined
                      rounded
                      v-on:keyup.enter="alterarSenha"
                      v-model="novaSenhaConfirmacao"
                      :rules="regrasSenha"
                      :type="showConfirmarSenha ? 'text' : 'password'"
                      :append-icon="showConfirmarSenha ? 'mdi-eye' : 'mdi-eye-off'"
                      @click:append="showConfirmarSenha = !showConfirmarSenha"
                    />
                  </v-col>
                </v-row>
              </v-container>
            </v-form>
          </v-card-text>
          <div class="pr-8 pl-8 text-center">
            <v-alert
              :value="alertConfirmacao"
              color="red"
              transition="scale-transition"
              text
            >
              <v-icon color="red" left>mdi-alert</v-icon>
              {{ mensagemAlteracao }}
            </v-alert>
          </div>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              color="red darken-1"
              @click="modal = false"
              text
              right
              class="mr-2 mb-4"
              >Cancelar</v-btn
            >
            <v-btn color="#3b71fe" @click="alterarSenha" text class="mr-4 mb-4"
              >Salvar</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog v-model="modalCadastrar" max-width="600px">
        <v-card>
          <v-toolbar dark color="primary">
            <v-toolbar-title>Cadastre-se</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon dark @click="ocultarModalCadastrar">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text>
            <v-form ref="formularioCadastro">
              <v-container>
                <v-row class="pt-2">
                  <v-col cols="12" xs="12" class="pb-2">
                    <v-text-field
                      v-model="cadastroNome"
                      label="Nome"
                      clearable
                      dense
                      :rules="[(v) => !!v || 'O campo nome é obrigatório']"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-2 pt-0">
                    <v-text-field
                      label="Login"
                      dense
                      v-model="cadastroLogin"
                      clearable
                      :error-messages="cadastroLoginDuplicado"
                      :rules="[(v) => !!v || 'O campo login é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-2 pt-0">
                    <v-text-field
                      label="Senha"
                      dense
                      v-model="cadastroSenha"
                      clearable
                      :type="showCadastroSenha ? 'text' : 'password'"
                      :append-icon="
                        showCadastroSenha ? 'mdi-eye' : 'mdi-eye-off'
                      "
                      @click:append="showCadastroSenha = !showCadastroSenha"
                      :rules="[(v) => !!v || 'O campo senha é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-2 pt-0">
                    <v-text-field
                      label="Confirmar senha"
                      dense
                      v-model="cadastroConfirmarSenha"
                      clearable
                      :type="showCadastroConfirmarSenha ? 'text' : 'password'"
                      :append-icon="
                        showCadastroConfirmarSenha ? 'mdi-eye' : 'mdi-eye-off'
                      "
                      @click:append="
                        showCadastroConfirmarSenha = !showCadastroConfirmarSenha
                      "
                      :error-messages="cadastroConfirmarSenhaErro"
                      :rules="[
                        (v) => !!v || 'O campo confirmar senha é obrigatório',
                      ]"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-2 pt-0">
                    <v-text-field
                      label="E-mail"
                      dense
                      v-model="cadastroEmail"
                      :error-messages="cadastroEmailDuplicado"
                      clearable
                      :rules="[(v) => !!v || 'O campo e-mail é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-2 pt-0">
                    <v-text-field
                      label="Confirmar e-mail"
                      dense
                      v-model="cadastroConfirmarEmail"
                      :error-messages="cadastroConfirmarEmailErro"
                      clearable
                      :rules="[
                        (v) => !!v || 'O campo confirmar e-mail é obrigatório',
                      ]"
                    />
                  </v-col>
                </v-row>
              </v-container>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              color="red darken-1"
              @click="ocultarModalCadastrar"
              text
              right
              class="mr-2 mb-4"
              >Cancelar</v-btn
            >
            <v-btn
              color="primary"
              @click="cadastrarUsuario"
              text
              class="mr-4 mb-4"
              >Salvar</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog content-class="rounded-xl" v-model="modalEsqueceuSenha" max-width="500px">
        <v-card rounded="xl">
          <v-toolbar dark color="#3b71fe">
            <v-toolbar-title>Esqueceu sua senha?</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon dark @click="ocultarEsqueceuSenha">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text>
            <v-form ref="formularioReset">
              <v-container>
                <v-row class="pt-2">
                  <v-col cols="12" xs="12" class="pb-0">
                    <p class="ma-0 text-justify">Para resetar sua senha informe seu login ou e-mail. Você receberá um e-mail solicitando a confirmação de alteração.</p>
                  </v-col>
                  <v-col cols="12" xs="12" class="pb-0 pt-6">
                    <v-text-field
                      v-model="usuarioOuEmailReset"
                      label="Login ou E-mail"
                      clearable
                      dense
                      outlined
                      rounded
                      height="60px"
                      class="alinhamentoIcon"
                      :error-messages="nenhumUsuarioEncontrado"
                      :rules="[(v) => !!v || 'O campo login ou e-mail é obrigatório']"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              color="red darken-1"
              @click="ocultarEsqueceuSenha"
              text
              right
              rounded
              class="mr-2 mb-4"
              >Cancelar</v-btn
            >
            <v-btn
              color="#3b71fe"
              @click="consultarUsuarioPorLoginOuEmail"
              text
              rounded
              class="mr-4 mb-4"
              >Resetar Senha</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
    <ModalSucesso
      :mensagem="mensagemAlerta"
      :alertaSucesso="alertaSucesso"
      :titulo="mensagemTitulo"
      @ocultarSucesso="ocultarSucesso"
    />
  </div>
</template>

<script>
import MensagemErro from "../components/mensagem-erro.vue";
import ModalSucesso from "../components/modal-sucesso.vue";
import Loading from "../components/loading.vue";
import UsuarioService from "../service/usuario-service.js";

const usuarioService = new UsuarioService();

export default {
  components: {
    Loading,
    MensagemErro,
    ModalSucesso,
  },
  data() {
    return {
      mensagemTitulo: null,
      alertaSucesso: false,
      loading: false,
      modal: false,
      valido: true,
      alert: false,
      alertaErro: false,
      mensagem: "",
      mensagemAlerta: "",
      alertConfirmacao: false,
      usuario: "",
      senha: "",
      novaSenhaConfirmacao: "",
      novaSenha: "",
      // regrasUsuario: [(v) => !!v || "Digite o usuário"],
      // regrasSenha: [(v) => !!v || "Digite a senha"],
      mensagemErroUsuario: null,
      mensagemErroSenha: null,
      mensagemAlteracao: "",
      showSenha: false,
      showNovaSenha: false,
      showConfirmarSenha: false,
      objLogin: null,
      modalCadastrar: false,
      cadastroNome: null,
      cadastroLogin: null,
      cadastroLoginDuplicado: null,
      cadastroEmail: null,
      cadastroEmailDuplicado: null,
      cadastroConfirmarEmail: null,
      cadastroConfirmarEmailErro: null,
      cadastroSenha: null,
      cadastroConfirmarSenha: null,
      cadastroConfirmarSenhaErro: null,
      showCadastroSenha: false,
      showCadastroConfirmarSenha: false,
      modalEsqueceuSenha: false,
      usuarioOuEmailReset: null,
      nenhumUsuarioEncontrado: null,
      acaoReset: false,
      routeId: null
    };
  },
  methods: {
    closeLoading() {
      setTimeout(() => {
          this.loading = false;
          this.mensagem = "O serviço demorou para responder";
          this.alert = true;
          setTimeout(() => {
            this.alert = false;
          }, 2000);
      }, 1000);
    },
    limparErro() {
      this.mensagemErroUsuario = null;
      this.mensagemErroSenha = null;
    },
    async consultarUsuarioPorLoginOuEmail(){
      this.nenhumUsuarioEncontrado = null;
      if (this.$refs.formularioReset.validate()) {
        // this.loading = true;

        // let result = await usuarioService.consultarUsuarioPorLoginOuEmail(
        //   this.usuarioOuEmailReset
        // );
        
        // if (result?.statusCode === 200) {
        //   this.mensagemAlerta =
        //     "Solicitação de reset de senha enviada com sucesso para o e-mail '" + result?.data.email + "'.";
        //   this.mensagemTitulo = "Reset de senha";
        //   this.alertaSucesso = true;
        //   this.loading = false;
        //   this.acaoReset = true;
        //   this.modalEsqueceuSenha = false;
        //   this.usuarioOuEmailReset = null;
        //   this.$refs.formularioReset.resetValidation();
        // } else if (result?.statusCode == 404){
        //   this.loading = false;
        //   this.nenhumUsuarioEncontrado = result?.data.mensagem;
        // } 
        // else {
        //   this.loading = false;
        //   this.mensagemAlerta = result?.data.mensagem;
        //   this.alertaErro = true;
        // }

        this.mensagemAlerta =
          "Solicitação de reset de senha enviada com sucesso para o e-mail 'exemplo@exemplo.com'.";
        this.mensagemTitulo = "Reset de senha";
        this.alertaSucesso = true;
        this.loading = false;
        this.acaoReset = true;
        this.modalEsqueceuSenha = false;
        this.usuarioOuEmailReset = null;
        this.$refs.formularioReset.resetValidation();
      }
    },
    exibirEsqueceuSenha(){
      this.modalEsqueceuSenha = true;
    },
    ocultarEsqueceuSenha(){
      this.modalEsqueceuSenha = false;
      this.$refs.formularioReset.resetValidation();
    },
    ocultarSucesso() {
      this.alertaSucesso = false;
      this.loading = false;
      if(!this.acaoReset)
        this.setLogin(this.objLogin);
    },
    async cadastrarUsuario() {
      if (this.$refs.formularioCadastro.validate()) {
        // this.cadastroLoginDuplicado = null;
        // this.cadastroEmailDuplicado = null;
        // this.cadastroConfirmarEmailErro = null;
        // this.cadastroConfirmarSenhaErro = null;

        // let valido = true;

        // if (this.cadastroEmail != this.cadastroConfirmarEmail) {
        //   this.cadastroConfirmarEmailErro =
        //     "Os e-mails informados não coincidem";
        //   valido = false;
        // }

        // if (this.cadastroSenha != this.cadastroConfirmarSenha) {
        //   this.cadastroConfirmarSenhaErro =
        //     "As senhas informadas não coincidem";
        //   valido = false;
        // }

        // if (valido) {
        //   this.loading = true;

        //   const cadastroUsuario = {
        //     nome: this.cadastroNome,
        //     senha: this.cadastroSenha,
        //     email: this.cadastroEmail,
        //     usuarioAD: this.cadastroLogin,
        //     ativo: true,
        //     primeiroAcesso: false,
        //   };

        //   let result = await usuarioService.cadastrarNovoUsuarioLogin(
        //     cadastroUsuario
        //   );
          
        //   if (result?.statusCode === 200) {
        //     this.loading = false;
        //     this.objLogin = result?.data;
        //     this.mensagemAlerta =
        //       "O usuário '" + this.cadastroLogin + "' foi criado com sucesso. Clique em OK para ser redirecionado à tela principal.";
        //     this.mensagemTitulo = "Criação de usuário";
        //     this.alertaSucesso = true;
        //     this.acaoReset = false;
        //     this.limparCadastroUsuario();
        //   } else {
        //     this.loading = false;
        //     if (result?.statusCode == 409) {
        //       if (result?.data.data.campoErro == "login") {
        //         this.cadastroLoginDuplicado = result?.data.mensagem;
        //       }
        //       if (result?.data.data.campoErro == "email") {
        //         this.cadastroEmailDuplicado = result?.data.mensagem;
        //       }
        //     } else {
        //       this.mensagemAlerta = result?.data.mensagem;
        //       this.alertaErro = true;
        //     }
        //   }
        // }
        this.loading = false;
        this.objLogin = {
            id: "123",
            nome: "Administrador",
            email: "admin@admin.com",
            ativo: true,
            login: "admin",
            senha: "",
            token: "",
            primeiroAcesso: false,
            ultimoAcesso: new Date(),
            dataUltimoToken: new Date(),
            perfilId: "",
            resetSolicitado: false,
            perfil: {
              id: "",
              descricao: "Admin",
              modulos: [
                {
                  id: "123",
                  titulo: "Módulo 1",
                  descricao: "Exemplo de módulo da aplicação",
                  url: "/modulos/pagina",
                  icone: "mdi-home-outline",
                  ordem: 1,
                  moduloPaginas: [
                    {
                      id: "123",
                      titulo: "Testar Hub",
                      descricao: "Exemplo de página para testar hub SignalR",
                      url: "/modulos/hub",
                      icone: "mdi-wifi",
                      card: true,
                      ordem: 1,
                      moduloId: ""
                    }
                  ]
                }
              ]
            }
          };
        this.mensagemAlerta =
          "O usuário '" + this.cadastroLogin + "' foi criado com sucesso. Clique em OK para ser redirecionado à tela principal.";
        this.mensagemTitulo = "Criação de usuário";
        this.alertaSucesso = true;
        this.acaoReset = false;
        this.limparCadastroUsuario();
      }
    },
    limparCadastroUsuario() {
      this.$refs.formularioCadastro.resetValidation();
      this.cadastroNome = null;
      this.cadastroLogin = null;
      this.cadastroLoginDuplicado = null;
      this.cadastroEmail = null;
      this.cadastroEmailDuplicado = null;
      this.cadastroConfirmarEmail = null;
      this.cadastroConfirmarEmailErro = null;
      this.cadastroSenha = null;
      this.cadastroConfirmarSenha = null;
      this.cadastroConfirmarSenhaErro = null;
      this.showCadastroSenha = null;
      this.showCadastroConfirmarSenha = null;
    },
    ocultarModalCadastrar() {
      this.modalCadastrar = false;
    },
    exibirModalCadastrar() {
      this.modalCadastrar = true;
    },
    async entrar() {
      let continuar = true;

      if(this.usuario?.length === 0) {
        this.mensagemErroUsuario = "Campo login obrigatório"
        continuar = false;
      }

      if(this.senha?.length === 0) {
        this.mensagemErroSenha = "Campo senha obrigatório"
        continuar = false;
      }

      if(continuar) {
        this.loading = true;
        //this.closeLoading();
        let result = await usuarioService.efetuarLogin(
          this.usuario,
          this.senha
        );
        if (result?.statusCode === 200) {
          if (result?.data.primeiroAcesso) {
            this.loading = false;
            this.modal = true;
            this.objLogin = result?.data;
          } else {
            this.loading = false;
            this.setLogin(result?.data);
          }
        } else {
          this.loading = false;
          if (result?.statusCode == 401) {
            this.mensagem = result?.data.mensagem;
            this.alert = true;
            setTimeout(() => {
              this.alert = false;
            }, 2000);
          } else {
            this.mensagemAlerta = result?.data.mensagem;
            this.alertaErro = true;
          }
        }
      }
    },
    async alterarSenha() {
      if (this.$refs.formularioAlteracao.validate()) {
        if (
          this.novaSenha === this.novaSenhaConfirmacao &&
          this.novaSenha !== this.objLogin.senha
        ) {
          let result = await usuarioService.alterarSenha(
            this.novaSenha,
            this.objLogin.token
          );
          if (result?.statusCode === 200) {
            this.loading = false;
            this.setLogin(this.objLogin);
          } else {
            this.loading = false;
            this.mensagemAlerta = result?.data.mensagem;
            this.alertaErro = true;
          }
        } else if (
          this.novaSenha === this.novaSenhaConfirmacao &&
          this.novaSenha === this.objLogin.senha
        ) {
          this.alertConfirmacao = true;
          this.mensagemAlteracao =
            "A nova senha deve ser diferente da anterior";
          setTimeout(() => {
            this.alertConfirmacao = false;
          }, 2000);
        } else {
          this.alertConfirmacao = true;
          this.mensagemAlteracao = "As senhas informadas não coincidem";
          setTimeout(() => {
            this.alertConfirmacao = false;
          }, 2000);
        }
      }
    },
    setLogin(objLogin) {
      const rotaNavegacao = localStorage.getItem("rotaNavegacao");

      localStorage.setItem("login", JSON.stringify(objLogin));
      if (rotaNavegacao) {
        localStorage.removeItem("rotaNavegacao");
        this.$router.push(rotaNavegacao).catch(failure => { localStorage.setItem("erro", failure) });
      } else this.$router.push("/modulos").catch(failure => { localStorage.setItem("erro", failure) });
    },
    async obterUsuarioReset(){
      this.loading = true;
      // let result = await usuarioService.getById(
      //   this.routeId
      // );
      // if (result?.statusCode === 200) {
      //   this.loading = false;
      //   this.objLogin = result?.data;
      //   if(this.objLogin.resetSolicitado)
      //     this.modal = true;
      // } else {
      //   this.loading = false;
      // }
    }
  },
  mounted: function () {
    document.getElementsByTagName("html")[0].style.overflow = "hidden";
    const routeId = this.$route.query.id;
    if(routeId){
      this.routeId = routeId;
      this.obterUsuarioReset();
    }
  },
};
</script>

<style scoped>
.logo {
  height: 300px;
  transition: all 1s;
}

.logo-background {
  top: 330px;
  left: 200px;
  transition: all 1s;
  position: fixed;
}

.loginESenha {
   font-family: 'Poppins'; 
   font-weight: 600; 
   font-size: 34px;
   color: #5D5D5F;
}

.login-bg {
  height: 100vh;
  width: 100vw;
  background-image: url(../assets/background_3.png);
  margin: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-card-container {
  position: relative;
  width: fit-content;
}

.login-card-icon {
  position: absolute;
  top: -32px;
  left: 0;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  display: none;
}

.login-card-icon i {
  border-radius: 100%;
  background: red;
  color: white;
  padding: 12px;
}

.login-card {
  width: 710px;
  height: 410px;
  overflow: hidden;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 1.25em;
  background-color: #f4f4f5;
}

.login-left-side {
  width: 60%;
  background-image: linear-gradient(175deg, #16ce8e, #16ce8e4d);
  transform: skew(-5deg);
  height: 100%;
  border-radius: 0 !important;
  margin-left: -30px;
  display: flex;
  align-items: start;
  justify-content: center;
}

.login-left-side-text {
  width: 400px;
  transform: skew(5deg);
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: start;
  margin-top: 40px;
}

.login-left-side-text img {
  margin-top: 30px;
  margin-bottom: 40px;
}

.title {
  /* color: #12466e; */
  font-size: 2em !important;
  margin-bottom: 12px;
}

.subtitle {
  /* color: #12466ed4; */
  font-size: 20px !important;
}

.login-right-side {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  flex-direction: column;
}
.login-header {
  margin: 30px 0 30px 60px;
  padding: 0;
  font-family: 'Poppins';
  font-size: 45px;
}
.login-form {
  width: 100%;
  padding-right: 24px;
}
.login-action {
  display: flex;
  align-items: center;
  width: 100%;
  justify-content: end;
  padding-right: 25px;
}
.login-action span {
  cursor: pointer;
  margin-right: 0px;
  margin: 0px 30px 20px 0px;
  font-weight: bold;
  color: #3b71fe;
}
.incorreto {
  width: 100%;
  display: initial;
  align-content: center;
  justify-content: center;
}
.cadastre-se {
  position: absolute;
  width: 40%;
  bottom: 0;
  right: 0;
  text-align: center;
  padding-bottom: 24px;
  color: #937a82;
}
.cadastre-se-button {
  /* color: #12466e; */
  font-size: 1.1rem;
  cursor: pointer;
}
.esqueceu-a-senha{
  font-size: 0.9rem;
}

.v-text-field {
  color: #3b71fe !important;
  font-family: 'Poppins' !important;
}

.v-card {
  border-radius: 0.5em;
  font-family: 'Poppins' !important;
}

.v-toolbar {
  border-radius: 0.5em;
  font-family: 'Poppins' !important;
}

.v-btn {
  font-family: 'Poppins' !important;
}

.text-justify {
  font-family: 'Poppins' !important;
}

.alinhamentoIcon >>> .v-label {
  margin-top: 10px
}

.alinhamentoIcon >>> .v-label--active {
  transform: translateY(-28px) scale(0.75);
}

@media (max-width: 1700px) {
  .logo {
    height: 300px;
    transition: all 1s;
  }

  .logo-background {
    top: 330px;
    left: 100px;
    transition: all 1s;
    position: fixed;
  }
}

@media (max-width: 1510px) {
  .logo-background {
    top: 330px;
    left: 5px;
    transition: all 1s;
    position: fixed;
  }
}

@media (max-width: 1200px) {
  .logo-background {
    top: 10px;
    left: 8px;
    position: fixed;
    transition: all 1s;
  }

  .logo {
    height: 180px;
    transition: all 1s;
  }
}

@media (max-width: 1020px) {
  .login-card {
    width: 800px;
  }
}

@media (max-width: 820px) {
  .login-card {
    width: 400px;
    height: 550px;
  }

  .login-action{
    display: inline-grid;
    justify-content: center;
    padding: 0 0 0 0;
  }

  .login-action span {
    display: flex;
    justify-content: center;
    margin: 0 0 10px 0;
  }

  .v-alert {
    margin: 0 0 10px 0;
  }

  .botao {
    display: flex;
    justify-content: center;
    margin: 0 0 10px 0;
  }

  .alerta {
    display: flex;
    justify-content: center;
  }

  .loginESenha {
    margin: 0 0 0 25px !important;
  }

  .login-header {
    margin: 0px 0 60px 25px !important;
  }

  .login-left-side {
    display: none;
    justify-content: space-evenly;
  }
  .login-right-side {
    width: 100%;
  }
  .login-form {
    padding-left: 0px;
  }
  .cadastre-se {
    width: 100%;
  }

  .v-text-field {
    text-align: end !important;
  }
}
</style>