<template>
  <v-card class="lwart-card pa-4" v-on:click="irParaLink()" elevation="8">
    <div class="lwart-card-title">
      <v-icon size="50px" color="#3b71fe" left>{{card.icone}}</v-icon>
      <span>{{card.titulo}}</span>
    </div>
    <div class="lwart-card-text" v-if="card.descricao">
      <v-card-text>{{card.descricao}}</v-card-text>
    </div>
  </v-card>
</template>

<script>
export default {
  props:["card", "novaAba"],
  name: 'CardSelecao',
  data() {
    return {};
  },
  methods: {
    irParaLink(){
      if(this.card.url){
        if(this.novaAba){
          window.open(this.card.url, "_blank")
        } else {
          this.$router.push(this.card.url).catch(failure => { localStorage.setItem("erro", failure) });
        }
      }
    }
  }
}
</script>
<style scoped>
.lwart-card span {
  font-family: 'Poppins';
  font-weight: 900;
  font-size: 40px !important;
  color: #5D5D5F;
}

.lwart-card {
  border-radius: 1.25em;
  background-color: #f4f4f5;
  transition: 0.3s ease;
  cursor: pointer;
  width: 550px;
  height: 200px;
  display: grid;
}
.lwart-card-title{
  display: flex;
  align-items: center;
  justify-content: center;
}
.lwart-card-title span{
  font-size: 20px;
}
.lwart-card-text{
  height: 75px;
  text-align: center;
}
.lwart-card:hover {
  transform: translateY(-5px);
  background-color: #e4effa;
}
.ultimoItem{
  margin-bottom: 50px;
}
</style>