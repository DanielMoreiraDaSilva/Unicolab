<template>
  <div>
    <v-divider class="mb-2"></v-divider>
    <div class="container-paginacao">
      <div>
        <div class="d-flex align-center responsive">
          Itens por página
          <v-select
            class="pagina-atual-select"
            v-model="itensPorPagina"
            :items="itensPagina ? itensPagina : [10, 20, 50, 100]"
            :menu-props="{ maxHeight: '400', offsetY: true, top: false }"
            label="Itens por página"
            single-line
            dense
            rounded
            flat
            solo
            hide-details
            @change="alterarItensPorPagina()"
            color="#3b71fe"
          ></v-select>
        </div>
      </div>
      <div class="d-flex align-center justify-flex-end">
        <div class="ml-4 mr-4 d-flex align-center">
          <div class="responsive">
            {{ totalItens == 0 ? 0 : !paginaAtual ? 0 : paginaAtual * itensPorPagina - itensPorPagina + 1 }} -
            {{
              paginaAtual * itensPorPagina > totalItens
                ? totalItens
                : paginaAtual * itensPorPagina
            }}
            de {{ totalItens }} itens |
          </div>
          <div style="width: 115px;">
            <v-select
              class="pa-0 ma-0"
              v-model="paginaAtual"
              :items="pAtuaisList"
              :menu-props="{ maxWidth: '100', maxHeight: '400', offsetOverflow: false, offsetY: true, top: true }"
              dense
              solo
              flat
              rounded
              hide-details
              @change="alterarPagina"
              ref="refPaginaAtual"
              color="#3b71fe"
            >
            </v-select>
          </div>
          <div class="pagina-atual-descricao">
            de {{ totalPaginas == 1 ? totalPaginas + " página" : totalPaginas + " páginas"}}
          </div>
        </div>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              icon
              color="#3b71fe"
              v-on="on"
              @click="primeiraPagina"
              class="btn-paginacao"
            >
              <v-icon>mdi-page-first</v-icon>
            </v-btn>
          </template>
          <span>Primeira página</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              icon
              color="#3b71fe"
              v-on="on"
              @click="paginaAnterior"
              class="btn-paginacao"
            >
              <v-icon>mdi-chevron-left</v-icon>
            </v-btn>
          </template>
          <span>Página anterior</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              icon
              color="#3b71fe"
              v-on="on"
              @click="proximaPagina"
              class="btn-paginacao"
            >
              <v-icon>mdi-chevron-right</v-icon>
            </v-btn>
          </template>
          <span>Próxima página</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn
              icon
              color="#3b71fe"
              v-on="on"
              @click="ultimaPagina"
              class="btn-paginacao"
            >
              <v-icon>mdi-page-last</v-icon>
            </v-btn>
          </template>
          <span>Última página</span>
        </v-tooltip>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  props: ["totalPaginas", "paginaAtual", "totalItens", "itensPagina"],
  name: "Paginacao",
  data() {
    return {
      itensPorPagina: this.itensPagina ? this.itensPagina[0] : 10,
      tPaginas: 1,
      pAtual: 1,
      buscaPaginaAtual: '',
    };
  },
  methods: {
    primeiraPagina() {
      this.paginaAtual = 1;
      this.$emit("alterarPagina", this.paginaAtual);
    },
    proximaPagina() {
      this.paginaAtual =
        this.paginaAtual < this.totalPaginas
          ? this.paginaAtual + 1
          : this.totalPaginas;
      this.$emit("alterarPagina", this.paginaAtual);
    },
    paginaAnterior() {
      this.paginaAtual =
        this.paginaAtual > 1 ? this.paginaAtual - 1 : this.paginaAtual;
      this.$emit("alterarPagina", this.paginaAtual);
    },
    ultimaPagina() {
      this.paginaAtual = this.totalPaginas;
      this.$emit("alterarPagina", this.paginaAtual);
    },
    alterarPagina() {
      this.$emit("alterarPagina", this.paginaAtual);
      this.$refs.refPaginaAtual.blur();
    },
    alterarItensPorPagina() {
      this.$emit("alterarItensPorPagina", this.itensPorPagina);
    },
  },
  watch: {
    totalPaginas(){
      if(this.totalPaginas < this.paginaAtual)
        this.primeiraPagina();
    }
  },
  computed: {
    pAtuaisList: function () {
      let list = [];
      for (let i = 1; i <= this.totalPaginas; i++) {
        list.push(i);
      }
      return list
    }
  }
};
</script>
<style scoped>
.btn-paginacao:hover {
  background-color: #e4effa;
  box-shadow: 0px 3px 1px -2px rgba(0, 0, 0, 0.2),
    0px 2px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12) !important;
  color: #3b71fe;
}
.container-paginacao {
  display: flex;
  align-items: center;
  justify-content: space-between;
  color: #3b71fe;
}
.pagina-atual-select {
  width: min-content;
  color: #3b71fe;
}
.pagina-atual-descricao {
  z-index: 1;
  color: #3b71fe;
}
.v-select {
  color: #3b71fe !important;
}
@media (max-width: 610px) {
    .responsive {
        display: none !important;
    }
}
</style>