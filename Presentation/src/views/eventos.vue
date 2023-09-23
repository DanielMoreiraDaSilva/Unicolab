<template>
  <div>
    <Loading v-if="loading" />
    <Header titulo="Eventos"  homeModulo="/modulos" tooltipModulo="Home"/>
    <div class="pa-6 app-container">
      <v-card 
        rounded="xl"
        elevation="4"
        class="pt-5 pb-5 card-superior"
        color="#F4F4F5"
      >
        <v-btn
          v-if="validaPerfil()"
          rounded
          outlined
          color="#3b71fe" 
          text
          x-large
          elevation="2"
          class="card-botao" 
          @click="clickInserirEvento"
        >
          <v-icon large left>mdi-plus</v-icon> Evento
        </v-btn>
        <v-btn 
          rounded
          outlined
          color="#3b71fe" 
          text
          x-large
          elevation="2"
          class="card-botao" 
          @click="modalFiltro = true"
        >
          <v-icon large left>mdi-filter-outline</v-icon> Filtrar
        </v-btn>
        <v-text-field
          label="Pesquise por eventos..."
          dense
          clearable
          outlined
          rounded
          hide-details
          append-icon="mdi-magnify"
          color="#3b71fe" 
          height="55px"
          class="alinhamentoIcon"
          v-model="filtro.titulo"
          @keypress.enter="listarEventos"
        />
      </v-card>
      <v-card 
        class="card-inferior"
        rounded="xl"
        elevation="4"
        color="#F4F4F5"
        max-width="100%"
      >
          <v-row dense class="lista-cards" v-if="eventos.length > 0">
            <v-col
              v-for="evento in eventos"
              :key="evento.id"
              cols="7"
              xl="3"
              lg="4"
              md="5"
              sm="6"
              xs="7"
              style="display: flex; justify-content: center;"
            >
              <v-card
                class="card-oportunidade"
                rounded="xl"
                elevation="4"
                color="#DEDEE0"
                width="100%"
                height="250px"
              >
                <v-img
                  @click="selecionarItem(evento)"
                  :src="imagemBackground(evento)"
                  class="white--text align-end"
                  style="cursor:pointer"
                  gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
                  height="180px"
                >
                  <v-card-title v-text="evento.titulo"></v-card-title>
                </v-img>

                <v-card-actions>
                  <div class="data-card"><p class="mb-0">{{evento.mesFormatado}}</p><p class="mb-0">{{evento.diaFormatado}}</p></div>
                  <h3 class="ml-4">{{evento.descricaoArea}}</h3>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="red"
                    rounded
                    outlined
                    fab
                    x-small
                    class="mr-1 botao-fechar"
                    @click="excluirItem(evento)"
                    v-if="validaEdicao(evento)"
                  >
                    <v-icon >mdi-trash-can-outline</v-icon>
                  </v-btn>
                  <v-btn
                    color="#3b71fe"
                    rounded
                    outlined
                    fab
                    x-small
                    class="mr-3"
                    @click="editarItem(evento)"
                    v-if="validaEdicao(evento)"
                  >
                    <v-icon >mdi-pencil-outline</v-icon>
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-col>
          </v-row>
          <v-row v-else>
            <v-col class="aviso-zero-eventos">
              <h4 style="color: red; font-family: Poppins">
                NÃO EXISTEM EVENTOS DISPONÍVEIS NO MOMENTO
              </h4>
            </v-col>
          </v-row>
          <v-row class="paginacao">
            <v-col>
              <Paginacao
                id="paginacao"
                style="width: 100%"
                :totalPaginas="totalPages"
                :paginaAtual="filtro.pagina"
                :totalItens="totalItems"
                :itensPagina="[12,24,48,106]"
                @alterarItensPorPagina="alterarItensPorPagina($event)"
                @alterarPagina="alterarPagina($event)"
              />
            </v-col>
          </v-row>
      </v-card>
    </div>
    <ModalSucesso
      :mensagem="mensagemAlerta"
      :alertaSucesso="alertaSucesso"
      :titulo="mensagemTitulo"
      @ocultarSucesso="alertaSucesso=false"
    />
    <MensagemErro
      :mensagemAlerta="mensagemAlerta"
      :alertaErro="alertaErro"
      @ocultarErro="alertaErro = false"
      :login="false"
    />
    <v-row justify="center" v-if="item">
      <v-dialog
        persistent
        content-class="rounded-xl"
        v-model="modalDadosMestre"
        fullscreen
      >
        <v-card rounded="xl">
          <v-toolbar dark="dark" color="#3b71fe">
            <v-toolbar-title>{{item.id ? "Editar Evento" : "Inserir Evento"}}
            </v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon="icon" dark="dark" @click="closeModalDadosMestre">
                <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-toolbar>
            <v-card-text>
              <v-form ref="formDadosMestre" class="pt-4">
                <v-row 
                  class="pt-2 pb-4"
                >
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Titulo"
                      v-model="item.titulo"
                      dense="dense"
                      clearable="clearable"
                      outlined="outlined"
                      rounded="rounded"
                      height="60px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo titulo é obrigatório']"/>
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-select
                        label="Curso"
                        item-text="descricao"
                        item-value="id"
                        :items="cursos"
                        v-model="item.cursoId"
                        dense
                        clearable
                        outlined
                        rounded
                        height="60px"
                        color="#3b71fe"
                        class="alinhamentoIcon"
                        :rules="[(v) => !!v || 'O campo curso é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Palestrante"
                      v-model="item.palestrante"
                      dense="dense"
                      clearable="clearable"
                      outlined="outlined"
                      rounded="rounded"
                      height="60px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo palestrante é obrigatório']"/>
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Local"
                      v-model="item.localEvento"
                      dense="dense"
                      clearable="clearable"
                      outlined="outlined"
                      rounded="rounded"
                      height="60px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo local é obrigatório']"/>
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-6 pb-0">
                    <DatePicker
                      @selecionarData="selecionarDataInicio($event)"
                      :obrigatorio="true"
                      :margem="'mr-1'"
                      model="dataHorarioInicio"
                      :retornarHorario="false"
                      :horarioInicial="false"
                      label="Data início"
                      :valorInicial="item.dataHorarioInicio"
                      :ocultarDetalhes="false"
                    />
                  </v-col>
                  <v-col 
                    cols="12" 
                    xs="12" 
                    class="pt-2 pl-0 pr-0 pb-0"
                  >
                    <v-text-field
                      label="Duração"
                      v-model="item.duracao"
                      dense
                      clearable="clearable"
                      outlined
                      rounded
                      height="60px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      style="font-family: Poppins"
                      suffix="Minutos"
                      :rules="[(v) => !!v || 'O campo duração é obrigatório']"/>
                  </v-col>
                  <v-col 
                    cols="12" 
                    xs="12" 
                    class="pt-2 pl-0 pr-0 pb-0"
                  >
                    <v-textarea
                      label="Descrição"
                      dense
                      clearable
                      outlined
                      rounded
                      v-model="item.descricao"
                      maxlength="4000"
                      counter="4000"
                      no-resize
                      height="150"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo descrição é obrigatório']"/>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
            <v-card-actions class="pt-0">
              <v-spacer></v-spacer>
              <v-btn
                color="red darken-1"
                text
                rounded
                class="botao-fechar"
                @click="closeModalDadosMestre">
                <v-icon small="small">mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                class="mr-6"
                color="#3b71fe"
                text
                rounded
                @click="salvarItem">
                <v-icon small="small">mdi-check</v-icon>
                {{ item.id ? "Salvar" : "Inserir" }}
              </v-btn>
            </v-card-actions>
          </v-card>
      </v-dialog>
  </v-row>
  <v-row justify="center">
    <v-dialog persistent content-class="rounded-xl" v-model="modalFiltro" max-width="800px">
      <v-card rounded="xl">
        <v-toolbar dark color="#3b71fe">
        <v-toolbar-title>Filtro</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn icon dark @click="modalFiltro = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
        </v-toolbar>
        <v-card-text>
          <v-form ref="formFiltro" class="pt-4">
            <v-row class="pt-4 pb-4">
              <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                <v-text-field
                  label="Título"
                  dense
                  clearable
                  outlined
                  rounded
                  hide-details
                  color="#3b71fe" 
                  height="55px"
                  class="alinhamentoIcon"
                  v-model="filtro.titulo"
                />
              </v-col>
              <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                <v-autocomplete
                  label="Curso"
                  item-text="descricao"
                  item-value="id"
                  :items="cursos"
                  v-model="filtro.listaCurso"
                  dense
                  clearable
                  outlined
                  rounded
                  multiple
                  chips
                  deletable-chips
                  height="60px"
                  color="#3b71fe"
                  class="alinhamentoIcon"
                  no-data-text="Nenhum curso encontrada"
                  hide-details
                />
              </v-col>
              <v-col cols="6" class="pt-2 pl-0 pr-0 pb-0 mr-0">
                <DatePicker
                  @selecionarData="selecionarData($event)"
                  :obrigatorio="false"
                  :margem="'mr-1'"
                  model="dataInicio"
                  :retornarHorario="false"
                  :horarioInicial="false"
                  label="Data início"
                  :valorInicial="filtro.dataInicio"
                  :ocultarDetalhes="true"
                />
              </v-col>
              <v-col cols="6" class=" pt-2 pl-0 pr-0 pb-0">
                <DatePicker
                  @selecionarData="selecionarData($event)"
                  :margem="'ml-1'"
                  :obrigatorio="false"
                  model="dataFim"
                  :retornarHorario="false"
                  :horarioInicial="false"
                  label="Data fim"
                  :valorInicial="filtro.dataFim"
                  :ocultarDetalhes="true"
                />
              </v-col>
              <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                <v-text-field
                  label="Local"
                  dense
                  clearable
                  outlined
                  rounded
                  hide-details
                  color="#3b71fe" 
                  height="55px"
                  class="alinhamentoIcon"
                  v-model="filtro.local"
                />
              </v-col>
              <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                <v-text-field
                  label="Palestrante"
                  dense
                  clearable
                  outlined
                  rounded
                  hide-details
                  color="#3b71fe" 
                  height="55px"
                  class="alinhamentoIcon"
                  v-model="filtro.palestrante"
                />
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
              @click="modalFiltro = false"
            >
              <v-icon small>mdi-close</v-icon>Cancelar
            </v-btn>
            <v-btn
              color="#3b71fe"
              text
              rounded
              @click="listarEventos() && (modalFiltro = false)"
            >
              <v-icon small>mdi-check</v-icon>
              Filtrar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
    <v-row justify="center" v-if="item">
      <v-dialog
        content-class="rounded-xl"
        v-model="modalDetalhesEvento"
        width="1000px"
      >
        <v-card rounded="xl">
          <v-img
            :src="imagemBackground(item)"
            class="white--text align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="180px"
          >
            <v-card-title style="font-size: 40px" v-text="item.titulo"></v-card-title>
          </v-img>
          <v-card-text class="mt-4">
            <div style="font-family: 'Poppins'; font-size: 16px"><h1 class="mb-8">{{item.descricaoArea}}</h1></div>
            <h2 class="mb-6">Tipo do evento : Palestra</h2>
            <h2 class="mb-6">Local do evento : {{item.localEvento}}</h2>
            <h2 class="mb-6">Palestrante : {{item.palestrante}}</h2>
            <h2 class="mb-6">Data/Hora início : {{item.dataHorarioInicioFormatado}}</h2>
            <h2 class="mb-6">Duração : {{item.duracao}} minutos</h2>
            <h2 class="mb-6">Pontos : {{item.pontos}}</h2>
            <h2 class="mb-6">Descricação : {{item.descricao}}</h2>
          </v-card-text>
          <v-card-actions class="pt-0">
            <v-spacer></v-spacer>
            <v-btn
              color="#3b71fe"
              text
              right
              rounded
              @click="modalDetalhesEvento = false"
            >
              <v-icon small>mdi-close</v-icon>Fechar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </div>
</template>
<script>
import Header from "../components/header.vue";
import EventoService from "../service/evento-service.js";
import CursoService from "../service/curso-service.js";
import Loading from "../components/loading.vue";
import ModalSucesso from "../components/modal-sucesso.vue";
import MensagemErro from "../components/mensagem-erro.vue";
import Paginacao from "../components/paginacao.vue";
import Vue from "vue";
import DatePicker from "../components/date-picker.vue";

const eventoService = new EventoService();
const cursoService = new CursoService();
const vue = new Vue();

export default {
  components: {
    Header,
    Loading,
    ModalSucesso,
    MensagemErro,
    Paginacao,
    DatePicker
  },
  data() {
    return {
      modalDetalhesEvento: false,
      alertaSucesso: false,
      cursos: [],
      item: {},
      modalDadosMestre: false,
      modalFiltro: false,
      loading: false,
      mensagemAlerta: false,
      mensagemTitulo: "",
      alertaErro: false,
      eventos: [],
      defaultInsertObject: {
        id: null,
        cursoId: null,
        usuarioId: null,
        titulo: null,
        tipoEventoId: null,
        descricao: null,
        pontos: 25,
        palestrante: null,
        localEvento: null,
        dataHorarioInicio: null,
        duracao: 0,
        ativo: true,
        descricaoArea: null
      },
      filtro: {
        todos: null,
        cursoId: null,
        titulo: null,
        listaTipoEventoId: [],
        localEvento: null,
        dataInicio: null,
        dataFim: null,
        pagina: 1,
        itensPagina: 10
      },
      paginas: [],
    };
  },
  methods: {
    async excluirItem(evento) {
      this.loading = true;
      let result = await eventoService.deletar(evento.id)
      if (result?.statusCode === 200) {
        this.mensagemAlerta =
          "O evento foi excluido com sucesso";
        this.mensagemTitulo = "Excluir evento";
        this.loading = false;
        this.alertaErro = false;
        this.alertaSucesso = true;
        this.closeModalDadosMestre();
        this.$refs.formDadosMestre.resetValidation();
        await this.listarEventos();
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    validaEdicao(item) {
      const login = JSON.parse(localStorage.getItem("login"));
      if(item.usuarioId === login.id) {
        return true;
      } else {
        return false;
      }
    },
    imagemBackground(item) {
      switch (item.areaId) {
        case vue.$globals.areaTI:
          return require('../assets/background_ti.jpg');
        case vue.$globals.areaSaude:
          return require('../assets/background_saude.jpg');
        case vue.$globals.areaEngenharia:
          return require('../assets/background_engenharia.jpg');
        default:
          return require('../assets/background_3.png');
      }
    },
    editarItem(item){
      console.log(item)
      this.item = JSON.parse(JSON.stringify(item));
      this.modalDadosMestre = true;
    },
    selecionarItem(item){
      this.item = JSON.parse(JSON.stringify(item));
      this.modalDetalhesEvento = true;
    },
    async salvarItem() {
      this.validado = true;
      if (this.$refs.formDadosMestre.validate()) {
        this.loading = true;
        const login = JSON.parse(localStorage.getItem("login"));
        this.item.usuarioId = login.id;
        this.item.tipoEventoId = '86DB6D0E-00F0-431F-A5A0-284D78DBBE28';
        this.item.duracao = parseInt(this.item.duracao)
        console.log(this.item);
        let result = this.item.id
        ? await eventoService.atualizar(this.item)
        : await eventoService.cadastrar(this.item);
        if (result?.statusCode === 200) {
          if (this.item.id) {
            this.mensagemAlerta =
            "O evento '" + this.item.titulo + "' foi editado com sucesso";
            this.mensagemTitulo = "Edição de evento";
          } else {
            this.mensagemAlerta =
            "O evento '" + this.item.titulo + "' foi inserido com sucesso";
            this.mensagemTitulo = "Inserção de evento";
          }
          this.alertaErro = false;
          this.alertaSucesso = true;
          this.closeModalDadosMestre();
          this.$refs.formDadosMestre.resetValidation();
          await this.listarEventos();
        } else {
          this.mensagemAlerta = result?.data.mensagem;
          this.alertaErro = true;
          this.loading = false;
        }
      }
    },
    selecionarDataInicio(event){
      this.item[event?.model] = event?.data;
    },
    selecionarData(event){
      this.filtro[event?.model] = event?.data;
    },
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
    clickInserirEvento() {
      this.item = JSON.parse(JSON.stringify(this.defaultInsertObject));
      this.modalDadosMestre = true;
    },
    closeModalDadosMestre() {
      this.item = JSON.parse(JSON.stringify(this.defaultInsertObject));
      this.$refs.formDadosMestre.resetValidation();
      this.modalDadosMestre = false
    },
    validaPerfil() {
      const login = JSON.parse(localStorage.getItem("login"));
      if(vue.$globals.perfilCoordenador === login.perfilId || vue.$globals.perfilAdmin === login.perfilId) {
        return true;
      } else {
        return false;
      }
    },
    alterarPagina(pagina) {
      this.filtro.pagina = pagina;
      this.listarEventos();
    },
    alterarItensPorPagina(itens) {
      this.filtro.itensPagina = itens;
      this.filtro.pagina = 1;
      this.listarEventos();
    },
    async listarEventos() {
      this.loading = true;
      let result = await eventoService.listarEventos(this.filtro);
      if (result?.statusCode === 200) {
        this.eventos = result?.data.lista;
        this.totalPages = result?.data.paginas;
        this.totalItems = result?.data.totalItens;
        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    }
  },
  created: function() {
    this.listarEventos();
    this.listarCursos();
  },
};
</script>
<style scoped>
.paginacao {
  width: 100%;
}
.card-superior {
  display: flex;
  align-items: center;
  margin-bottom: 30px;
}
.card-inferior {
  padding: 30px 20px 30px 20px;
  min-height: 660px;
  height: 100%;
  display: grid;
  justify-content: center;
  grid-template-rows: 90% 10%;
  grid-template-columns: 100%;
}
.card-inferior-colunas {
  min-width: 20% !important;
  display: grid;
  align-content: center;
  justify-content: left;
}
.lista-cards {
  justify-content: left;
}
.card-botao {
  border: #3b71fe solid 2px !important;
  margin: 0 0 0 30px;
}
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
.v-btn {
  font-family: 'Poppins' !important;
  font-weight: 600;
}
.h1 {
  font-family: 'Poppins' !important;
}
.span {
  font-family: 'Poppins' !important; 
  font-weight: 600 !important;
}
.data-card {
  border: 2px solid rgb(0,0,0,0.6);
  display: grid;
  justify-items: center;
  align-content: center;
  border-radius: 10px;
  margin-left: 10px;
  width: 60px;
  height: 50px;
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
.aviso-zero-eventos {
  display: flex; 
  justify-content: center;
  align-items: center;
}
@media (max-width: 959px) {
  .lista-cards {
    margin-bottom: 80px;
    justify-content: center;
  }
  .paginacao {
    display: flex;
    align-content: center;
    align-items: end;
  }
}
@media (max-width: 1263px) {
  .lista-cards {
    margin-bottom: 80px;
    justify-content: center;
  }
  .paginacao {
    display: flex;
    align-content: center;
    align-items: end;
  }
}
</style>