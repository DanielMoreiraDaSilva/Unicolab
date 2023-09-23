<template>  
  <div>
    <div>
      <v-app-bar color="white" light height="120px">
        <div class="imagem-modulo">
            <img
              src="@/assets/exemplo-logo.jpg"
              width="255px"
              class="d-none d-sm-flex"
            />
        </div>
        <div class="lwart-divisor"></div>
        <v-toolbar-title class="titulo-header ml-3">
            <v-icon class="d-xs-none">
              mdi-chevron-right
            </v-icon>
            <span class="d-xs-none titulo-navegacao">
              Não Autorizado
            </span>
        </v-toolbar-title>
        <v-spacer></v-spacer>
      </v-app-bar>
    </div>
    <div>
        <div>
          <v-row style="justify-content: center" class="mt-6">
              <div class="card">
                    <div class="circulo">
                      <v-icon x-large
                              color="#ef5350">
                              mdi-lock
                      </v-icon>
                    </div>
                    <br>
                    <span class="acesso-negato-text">
                      Acesso Não Autorizado
                    </span>
                    <span>
                      Seu perfil não possui permissão para acessar está página.
                    </span>
                    <span>
                      Entre em contato com o Administrador,
                        click no botão para voltar a home.
                    </span>
                    <br>
                    <v-btn class="botao-home" color="primary" @click="home">
                      Home
                    </v-btn>
              </div>
          </v-row>
        </div>
      </div>
  </div>
</template>

<script>

import router from '../router/index';

export default {
  components: {
  },
  data(){
      return{
      };
  },
  methods:{
    home(){
      router.push("/modulos").catch(failure => { localStorage.setItem("erro", failure) });
    }
  }
}
</script>

<style scoped>
.acesso-negato-text{
  color: #ef5350;
  font-size: 35px;
}
.mb-4{
  text-align: center;
}
.mt-6{
  display: flex;
}
.pa-6{
  height: 100%;
}
.card{
  justify-content: center;
  display: flex;
  align-items: center;
  text-align: center;
}
.circulo{
  width: 100px;
  height: 100px;
  border-radius: 50%;
  background-color: #fbd1d0;
  margin: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
}
.lwart-divisor {
  background-image: linear-gradient(270deg, #8fc73e, #12466e);
  width: 100%;
  height: 10px;
  position: absolute;
  bottom: 0;
  left: 0;
}
</style>