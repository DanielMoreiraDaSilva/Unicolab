<template>
  <div>
    <Header titulo="Perfil"  homeModulo="/modulos" tooltipModulo="Home"/>
    <div class="pa-6 app-container d-flex">
      <v-card outlined elevation="10" rounded="xl" width="1000px">
        <p class="titulo"><v-icon size="60px" class="icone-titulo">mdi-account</v-icon> Meu Perfil</p>
        <v-form ref="formDadosMestre" class="pt-4">
          <v-row class="pt-2 pb-4">
            <v-col cols="12" xs="12" class="label">
              <v-text-field
                label="Nome Completo"
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.nome"
              />
            </v-col>
            <v-col cols="12" xs="12" class="label">
              <v-text-field
                label="Login"
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.login"
              />
            </v-col>
            <v-col cols="12" xs="12" class="label">
              <v-text-field
                label="Email"
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.email"
              />
            </v-col>
            <v-col cols="12" xs="12" class="label" v-if="validaPerfil()">
              <v-text-field
                label="Curso"
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.cursoDescricao"
              />
            </v-col>
            <v-col cols="12" xs="12" class="label pb-8" v-if="validaPerfil()">
              <v-text-field
                label="R.A."
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.ra"
              />
            </v-col>
            <v-col cols="12" xs="12" class="label pb-8" v-if="!validaPerfil()">
              <v-text-field
                label="Área"
                dense
                outlined
                rounded
                readonly
                height="60px"
                background-color="#F4F4F5"
                v-model="usuario.areaDescricao"
              />
            </v-col>
          </v-row>
        </v-form>
      </v-card>
    </div>
  </div>
</template>
<script>
import Header from "../components/header.vue";
import UsuarioService from "../service/usuario-service.js";
import Vue from "vue";

const vue = new Vue();

const usuarioService = new UsuarioService();

export default {
  components: {
    Header
  },
  data() {
    return {
      paginas: [],
      usuario: null,
    };
  },
  methods: {
    validaPerfil() {
      const login = JSON.parse(localStorage.getItem("login"));
      if(login.perfilId == vue.$globals.perfilAluno) {
        return true;
      } else {
        return false;
      }
    },
    async getUsuario() {
      let usuarioLogado = JSON.parse(localStorage.getItem("login"));
      let result = await usuarioService.getById(usuarioLogado.id);
      this.usuario = result.data;
      if(this.usuario.perfilId == vue.$globals.perfilAdmin) {
        this.usuario.areaDescricao = 'Todas as areas'
      }
      this.usuario.ra = '1234567890'
    }
  },
  created: function() {
    this.getUsuario();
  },
};
</script>

<style scoped>
.app-container {
  justify-content: center;
  display: flex;
}
.v-text-field {
  color: #3b71fe !important; 
  font-family: 'Poppins';
  font-weight: normal;
}

.label {
  padding: 20px 60px 0px 60px ;
}

.titulo {
  font-weight: bolder;
  font-size: 40px;
  font-family: 'Poppins';
  padding: 40px 60px 0px 60px ;
}

.v-card {
  border: 1px solid;
}

.icone-titulo {
  color: #3b71fe;
}
</style>