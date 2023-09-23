<template>
  <div @click="opcoesPerfil = false">
    <Loading v-if="loading" />
    <v-app-bar color="#f4f4f5" height="120px" elevation="0">
      <v-tooltip bottom v-if="!paginaInicial">
        <template v-slot:activator="{ on }">
          <div v-on="on" class="mr-4 btn-voltar" v-on:click="voltar">
            <v-icon size="30px">mdi-arrow-left</v-icon>
          </div>
        </template>
        <span style="font-family: Poppins; font-weight: 600;">Voltar</span>
      </v-tooltip>
      <v-tooltip bottom>
        <template v-slot:activator="{ on }">
          <div class="imagem-modulo" v-on="on" @click="irParaLink('/modulos')">
            <img
              src="@/assets/logo.png"
              width="100px"
              class="d-none d-sm-flex"
            />
          </div>
        </template>
        <span style="font-family: Poppins; font-weight: 600;">Home</span>
      </v-tooltip>
      <v-toolbar-title class="titulo-header ml-3">
        <v-icon
          class="d-xs-none"
          v-if="$route.path !== homeModulo && $route.path !== '/modulos'"
          >mdi-chevron-right</v-icon
        >
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <span
              v-on="on"
              class="d-xs-none titulo-navegacao"
              @click="irParaHomeModulo"
              v-if="$route.path !== homeModulo && $route.path !== '/modulos'"
              style="font-family: Poppins; font-weight: 600;"
              >{{ tooltipModulo }}</span
            >
          </template>
          <span style="font-family: Poppins; font-weight: 600;">Ir para página {{ tooltipModulo }}</span>
        </v-tooltip>

        <v-icon class="d-xs-none">mdi-chevron-right</v-icon>
        <span style="font-family: Poppins; font-weight: 600;">{{ titulo }}</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>

      <v-menu
        offset-y
      >
        <template v-slot:activator="{ on, value }">
          <v-btn
            :class="value ? 'ma-2 button-dark' : 'ma-2'"
            v-on="on"
            elevation="0"
            width="auto"
            height="70px"
            @focus="onFocus"
          >
            <div v-if="usuario">
              <span class="d-none d-sm-inline" style="font-family: Poppins;">{{usuario.nome}}</span>
              <v-icon class="pl-4" size="29px" :color="value ? 'white' : '#3b71fe'">mdi-account</v-icon>
            </div>
          </v-btn>
        </template>
        <v-list>
          <v-list-item
            v-for="(item, i) in [{title: 'Meu Perfil', color: 'white', icone: 'mdi-account', click: 'perfil'}, {title: 'Sair', color: '#ff8989', icone: 'mdi-logout', click: 'logout'}]"
            :key="i"
            @click="clickOpcoesPerfil(item.click)"
          >
            <v-list-item-title class="pl-3" :style="'font-family: Poppins; color:' + item.color" >
              {{ item.title }}
              <v-icon class="pl-4" size="29px" :color="item.color">{{item.icone}}</v-icon>
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>

    </v-app-bar>

  </div>
</template>
<script>
import Loading from "../components/loading.vue";

export default {
  components: {
    Loading,
  },
  name: "Header",
  props: ["titulo", "homeModulo", "tooltipModulo"],
  data() {
    return {
      drawer: null,
      showDrawer: null,
      paginaInicial: false,
      sair: false,
      loading: false,
      usuario: {
        nome: '',
        perfil:{
          modulos:[]
        }
      },
    };
  },
  watch: {
    drawer() {
      document.getElementsByTagName("html")[0].style.width = "auto";
      if (!this.drawer) {
        setTimeout(() => {
          this.showDrawer = false;
          document.getElementsByTagName("html")[0].style.width = "100vw";
          document.getElementsByTagName("html")[0].className = "";
        }, 100);
      } else {
        setTimeout(() => {
          document.getElementsByTagName("html")[0].style.width = "100vw";
          document.getElementsByTagName("html")[0].className = "";
        }, 200);
      }
    },
  },
  methods: {
    abrirPerfil() {
      this.loading = true;
      this.$router.push('/perfil').catch(failure => { localStorage.setItem("erro", failure) });
      this.loading = false;
    },
    clickOpcoesPerfil(click) {
      if(click == 'logout') {
        this.logout();
      } else if (click == 'perfil') {
        this.abrirPerfil();
      }
    },
    onFocus (e) {
      e.target.blur()
    },
    voltar() {
      history.back();
    },
    abrirModalSair() {
      this.sair = true;
    },
    fecharModalSair() {
      this.sair = false;
      this.showDrawer = true;
    },
    logout() {
      this.loading = true;
      localStorage.removeItem("login");
      this.$router.push("/").catch(failure => { localStorage.setItem("erro", failure) });
      this.loading = false;
    },
    irParaHomeModulo() {
      if (this.$route.path !== this.homeModulo)
        this.$router.push(this.homeModulo).catch(failure => { localStorage.setItem("erro", failure) });
    },
    irParaLink(url) {
      if (this.$route.path !== url) this.$router.push(url).catch(failure => { localStorage.setItem("erro", failure) });
      else this.drawer = false;
    },
  },
  mounted: function () {
    const modulo = location.href.split("/modulos").length == 2;
    const rotaDireta = location.href.split("?id=").length == 2;
    this.paginaInicial = modulo || rotaDireta;
    let login = JSON.parse(localStorage.getItem("login"));
    this.usuario = login;
  },
};
</script>
<style scoped>
.span {
  font-family: 'Poppins';
  font-weight: 600; 
}

.btn-voltar {
  cursor: pointer;
  border-radius: 100%;
  padding: 5px;
}
.btn-voltar i {
  color: unset;
}
.imagem-modulo {
  cursor: pointer;
}
.titulo-header {
  font-size: 25px;
  cursor: default;
}
.titulo-navegacao {
  cursor: pointer;
}
.titulo-navegacao:hover {
  color: #12466e !important;
}

.v-btn {
  border-radius: 40px;
  border: 2px solid #3b71fe;
  padding: 20px 20px 20px 25px;
  cursor: pointer;
  transition: 0.5s ease;
  color: #3b71fe;
}

.v-btn:before {
  background: #f4f4f5
}

.v-menu__content {
  box-shadow: 0 0 0 0;
}

.theme--light.v-list {
  background: transparent;
  box-shadow: 0;
  display: grid;
  justify-items: end;
}

.theme--light.v-list-item:not(.v-list-item--active):not(.v-list-item--disabled) {
  color: white;
  border-radius: 40px;
  background: #3b71fe;
  margin-bottom: 8px;
  margin-right: 10px;
  height: 70px;
  width: auto;
  text-align: end;
  font-family: 'Poppins';
}

.button-dark {
  background-color: #3b71fe !important;
  color: white;
}

.btn-voltar:hover {
  background: rgba(255, 255, 255, 0.1);
  box-shadow: 0px 3px 1px -2px rgba(0, 0, 0, 0.2),
    0px 2px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12) !important;
  color: #12466e !important;
}
</style>