<template>
  <div>
    <v-menu
      v-model="menuData"
      :close-on-content-click="false"
      transition="scale-transition"
      offset-y
      max-width="290px"
      min-width="290px"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-text-field
          :class="margem + ' alinhamentoIcon'"
          append-icon="mdi-calendar"
          v-bind="attrs"
          v-model="dataFormatada"
          :label="label"
          v-on="on"
          clearable
          :outlined="withoutOutline ? false : true"
          dense
          rounded
          color="#3b71fe" 
          height="55px"
          :rules="rules"
          v-mask="['##/##/####']"
          :error-messages="erroPersonalizado"
          @click:clear="data = null"
          v-on:keyup="validarTamanho()"
          v-on:click:append="menuData=true"
          :hide-details="ocultarDetalhes == true ? true : false"
        ></v-text-field>
      </template>
      <v-date-picker v-model="data" @input="menuData = false" locale="pt-br" no-title></v-date-picker>
    </v-menu>
  </div>
</template>
<script>
import { mask } from "vue-the-mask";

export default {
  directives: {
    mask
  },
  props: [
    "obrigatorio",
    "model",
    "retornarHorario",
    "horarioInicial",
    "label",
    "erroPersonalizado",
    "valorInicial",
    "ocultarDetalhes",
    "mensagemCampoObrigatorio",
    "withoutOutline",
    "margem"
  ],
  data: vm => ({
    data: vm.getValorInicial(),
    dataFormatada: vm.formatarData(vm.getValorInicial()),
    menuData: false,
    dataRetornada: false
  }),
  methods: {
    getValorInicial(){
      if(this.valorInicial)
        return JSON.parse(JSON.stringify(this.valorInicial.substr(0, 10)))
      else
        return null;
    },
    validarTamanho(){
      setTimeout(() => {
        if(this.dataFormatada?.length == 10)
          this.converterData(this.dataFormatada);
      }, 100);
    },
    retornarData(){
      let horario = "";
      if(this.retornarHorario && this.data){
        if(this.horarioInicial)
          horario = "T00:00:00.000";
        else
          horario = "T23:59:59.000";
      }    
      const retorno = {
        model: this.model,
        data: this.data ? this.data + horario.toString() : null
      }
      this.dataRetornada = true;
      this.$emit("selecionarData", retorno);
    },
    formatarData(data) {
      if (!data) return null;

      const [ano, mes, dia] = data.split("-");
      return `${dia}/${mes}/${ano}`;
    },
    async converterData(data) {
      if (!data){
        this.data = null;
        await this.retornarData();
        return;
      }
      let matches = /(\d{2})[-./](\d{2})[-./](\d{4})/.exec(data);
      if (matches == null) {
        this.data = null;
        await this.retornarData();
        return;
      }
      var day = parseInt(matches[1]);
      var month = parseInt(matches[2]);
      var year = parseInt(matches[3]);
      if (this.isValidDate(day, month, year)) {
        const [dia, mes, ano] = data.split("/");
        this.data = `${ano}-${mes.padStart(2, "0")}-${dia.padStart(2, "0")}`;
        await this.retornarData();
        return;
      } else {
        this.data = null;
        this.dataFormatada = null;
        await this.retornarData();
        return;
      }      
    },
    daysInMonth(m, y) {
      switch (m) {
        case 1:
          return (y % 4 == 0 && y % 100) || y % 400 == 0 ? 29 : 28;
        case 8:
        case 3:
        case 5:
        case 10:
          return 30;
        default:
          return 31;
      }
    },
    isValidDate(d, m, y) {
      m = parseInt(m, 10) - 1;
      return m >= 0 && m < 12 && d > 0 && d <= this.daysInMonth(m, y);
    }
  },
  watch: {
    data() {
      this.dataFormatada = this.formatarData(this.data);
      this.converterData(this.dataFormatada);
    }
  },
  computed: {
    computedDataFormatada() {
      return this.formatarData(this.data);
    },
    rules() {
      const rules = [];

      if(this.obrigatorio)
      {
        const rule = v => !!v || (this.mensagemCampoObrigatorio ? this.mensagemCampoObrigatorio : 'O campo data é obrigatório');

        rules.push(rule);
      }
      return rules;
    }
  },
  beforeDestroy: function(){
    if(!this.dataRetornada)
      this.retornarData();
  }
};
</script>
<style scoped>
.alinhamentoIcon {
  margin: 0 30px 0 30px;
  font-family: 'Poppins';
  font-weight: 600;
}
.alinhamentoIcon >>> .v-input__append-inner buttom {
  margin-top: 18px !important;
}
.alinhamentoIcon >>> .v-input__icon {
  margin-top: 0 !important;
  min-height: 40px;
}
.alinhamentoIcon >>> .v-label {
  margin-top: 10px;
}
.alinhamentoIcon >>> .v-label--active {
  transform: translateY(-28px) scale(0.75);
}
</style>