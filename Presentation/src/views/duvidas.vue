<template>
  <div>
    <Loading v-if="loading" />
    <Header titulo="Dúvidas"  homeModulo="/modulos" tooltipModulo="Home"/>
    <div class="pa-6 app-container">
      <div class="container-esquerdo">
        <v-card 
          rounded="xl"
          elevation="4"
          class="card-lateral"
          color="#F4F4F5"
        >
          <div class="card-lateral-info">
            <div class="mt-4 card-lateral-titulo">
              <div class="card-lateral-icon">
                <v-icon size="60px" class="icone-titulo">mdi-account-circle-outline</v-icon>
              </div>
              <p>
                <b>
                  {{usuario.nome}}
                </b>
              </p>
            </div>
            <div class="card-lateral-subtitulo" v-if="cursosAtivos.length > 0">
              <p>Curso</p>
              <div v-for="curso in cursosAtivos" :key="curso.id"><p>{{curso.descricao}}</p></div>
            </div>
            <div class="card-lateral-pontos" v-if="validaPerfilAluno()">
              <p>Pontos : {{usuario.pontos}}</p>
            </div>
            <div>
              <v-btn 
                rounded
                outlined
                color="#3b71fe" 
                text
                x-large
                dark
                elevation="2"
                :class="'card-lateral-botao' + (filtro.minhasDuvidas == true ? ' button-dark' : '')"  
                @click="minhasDuvidas"
              >
                Minhas Dúvidas
              </v-btn>
            </div>
          </div>
        </v-card>
      </div>
      <div  class="container-direito"> 
        <v-card 
          rounded="xl"
          elevation="4"
          class="mb-6 card-superior response"
          color="#F4F4F5"
        >
          <v-icon size="60px" class="ml-4 icone-titulo">mdi-account-circle-outline</v-icon>
          <div class="ml-4 card-lateral-pontos" v-if="usuario.pontos">
            <p>Pontos : {{usuario.pontos}}</p>
          </div>
          <div>
            <v-btn 
              rounded
              outlined
              color="#3b71fe" 
              text
              x-large
              elevation="2"
              :class="'ml-4 card-lateral-botao' + (filtro.minhasDuvidas == true ? ' button-dark' : '')" 
              @click="minhasDuvidas"
            >
              Minhas Dúvidas
            </v-btn>
          </div>
        </v-card>
        <v-card 
          rounded="xl"
          elevation="4"
          class="card-superior"
          color="#F4F4F5"
        >
          <v-btn 
            rounded
            outlined
            color="#3b71fe" 
            text
            x-large
            elevation="2"
            class="card-superior-botao" 
            @click="clickInserirDuvida"
          >
            <v-icon large left>mdi-plus</v-icon> Dúvida
          </v-btn>
          <v-btn 
            rounded
            outlined
            color="#3b71fe" 
            text
            x-large
            elevation="2"
            class="card-superior-botao" 
            @click="modalFiltro = true"
          >
            <v-icon large left>mdi-filter-outline</v-icon> Filtrar
          </v-btn>
          <v-text-field
            label="Pesquise por dúvidas..."
            dense
            clearable
            outlined
            rounded
            hide-details
            append-icon="mdi-magnify"
            color="#3b71fe" 
            height="55px"
            class="alinhamentoIcon"
            v-model="filtro.pergunta"
            @keypress.enter="listarDuvidas(false)"
            @keyup="validaBotaoPesquisa"
            @click:clear="botaoPesquisa = true"
          > 
            <template v-slot:append>
              <v-btn 
                outlined
                fab
                small
                color="#3b71fe" 
                text
                elevation="2"
                :disabled="botaoPesquisa"
                :class="'ml-0' + (botaoPesquisa ? ' ' : ' card-superior-botao')" 
                @click="listarDuvidas(false)"
              >
                <v-icon>mdi-magnify</v-icon>
              </v-btn>
            </template>
          </v-text-field>
        </v-card>
        <v-card 
          rounded="xl"
          elevation="4"
          class="card-inferior"
          color="#F4F4F5"
        >
          <v-list class="card-inferior-lista" v-if="duvidas.length > 0">
            <template v-for="(duvida, index) in duvidas">
              <v-list-tile :key="index">
                <v-list-tile-content>
                    <v-card 
                      :class="(index == 0 ? 'mt-0 ' : '') + 'card-inferior-duvida'"
                      rounded="xl"
                      elevation="4"
                      color="#DEDEE0"
                    >
                        <v-toolbar flat color="transparent">
                          <v-toolbar-title>
                            <div class="card-inferior-duvida-titulo">
                              <v-icon size="30px" class="mr-2 icone-titulo">mdi-help-circle-outline</v-icon>
                              <div class="mr-4">Matéria : {{ duvida.nomeMateria }}</div>
                            </div>
                          </v-toolbar-title>
                          <v-spacer></v-spacer>
                          <div v-if="validaIconeMelhorResposta(duvida.respostas)">
                            <v-tooltip content-class="span-pontos" bottom >
                              <template v-slot:activator="{ on }">
                                <div class="d-flex" style="justify-content: center;align-items: center;height: 50px;width: 50px;" v-on="on">
                                  <div class="mr-0 mt-2" 
                                    style="width: 30px;
                                    height: 30px;
                                    border-radius: 50%;
                                    background-color: white;
                                    color: white;
                                    position: relative"    
                                  >
                                  </div>
                                  <v-icon 
                                    style="position: absolute; "
                                    color="#eead2d"
                                    size="55px" 
                                    class="mr-0 mt-2"
                                  >
                                    mdi-check-decagram
                                  </v-icon>
                                </div>
                              </template>
                              <span class="span">Concluída</span>
                            </v-tooltip>
                          </div>
                          <v-tooltip content-class="span-pontos" bottom v-else>
                            <template v-slot:activator="{ on }">
                              <div class="mt-3 card-inferior-duvida-titulo-pontos" v-on="on">
                                {{duvida.pontos}}
                              </div>
                            </template>
                            <span class="span">Pontos</span>
                          </v-tooltip>
                        </v-toolbar>
                      <v-card-text>
                        <h2>{{ duvida.pergunta }}</h2>
                      </v-card-text>
                      <v-card-actions>
                        <v-icon class="ml-2 mr-2">mdi-account-edit-outline</v-icon>
                        <h5 style="color: rgba(0, 0, 0, 0.6);">{{ duvida.nomeUsuario + '  ' + '(' + duvida.dataHoraFormatada + ')'}}</h5>
                        <v-spacer></v-spacer>
                        <span class="mr-4">
                          {{ duvida.quantidadeResposta }} Resposta{{(duvida.quantidadeResposta == 1 ? '' : 's')}}
                        </span>
                        <v-btn
                            color="red"
                            rounded
                            outlined
                            fab
                            x-small
                            class="mr-2 card-inferior-duvida-botao-fechar"
                            @click="excluirItem(duvida)"
                            v-if="validaExclusao(duvida)"
                          >
                            <v-icon >mdi-trash-can-outline</v-icon>
                          </v-btn>
                        <v-btn
                          v-if="duvida.usuarioId != usuario.id"
                          outlined
                          color="#3b71fe" 
                          text
                          elevation="2"
                          rounded right 
                          class="card-inferior-duvida-botao"
                          @click="botaoResponderCard(duvida)"
                        >
                          <v-icon class="mr-2"> {{!validaBotaoCardDuvida(duvida.respostas) ? 'mdi-comment-arrow-left-outline' : 'mdi-eye-outline'}}</v-icon> 
                          {{!validaBotaoCardDuvida(duvida.respostas) ? 'Responder' : 'Visualizar'}}
                        </v-btn>
                        <v-btn
                          v-else
                          outlined
                          color="#3b71fe" 
                          text
                          elevation="2"
                          rounded right 
                          class="card-inferior-duvida-botao"
                          @click="duvida.respostas.length === 0 ? selecionarItem(duvida) : botaoResponderCard(duvida)"
                        >
                          <v-icon class="mr-2"> {{ duvida.respostas.length === 0 ? ' mdi-pencil-outline' : 'mdi-eye-outline'}}</v-icon> 
                          {{ duvida.respostas.length === 0 ? 'Editar' : 'Visualizar'}}
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <v-card 
              class="text-center card-inferior-duvida"
              rounded="xl"
              elevation="4"
              color="#3b71fe"
              v-if="carregarMais"
              @click="carregarMaisDuvidas"
            > <h3 style="color: white"> Carregar Mais </h3> </v-card>
          </v-list>
          <v-card-text class="aviso-zero-duvidas" v-else>
            <h3 style="color: red; font-family: Poppins">
              NÃO EXISTEM DÚVIDAS DISPONÍVEIS NO MOMENTO
            </h3>
          </v-card-text>
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
        <v-dialog persistent content-class="rounded-xl" v-model="modalDadosMestre" max-width="1000px">
          <v-card rounded="xl">
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>{{
                item.id ? "Editar Pergunta" : "Inserir Pergunta"
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
                    <v-select
                      label="Matéria"
                      item-text="descricao"
                      item-value="id"
                      :items="materias"
                      v-model="item.materiaId"
                      dense
                      clearable
                      outlined
                      rounded
                      height="60px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo matéria é obrigatório']"
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-textarea
                      label="Descrição"
                      dense
                      clearable
                      outlined
                      rounded
                      v-model="item.pergunta"
                      maxlength="4000"
                      counter="4000"
                      no-resize
                      height="400px"
                      color="#3b71fe"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo pergunta é obrigatório']"
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
                class="card-inferior-duvida-botao-fechar"
                @click="closeModalDadosMestre"
              >
                <v-icon small>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                class="card-inferior-duvida-botao"
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
        <v-dialog persistent content-class="rounded-xl" v-model="modalFiltro" max-width="800px">
          <v-card rounded="xl">
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>Filtro</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-btn icon dark @click="closeModalFiltro">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </v-toolbar>
            <v-card-text>
              <v-form ref="formFiltro" class="pt-4">
                <v-row class="pt-4 pb-4">
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-autocomplete
                      label="Matéria"
                      item-text="descricao"
                      item-value="id"
                      :items="materias"
                      v-model="filtro.listaMateria"
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
                      no-data-text="Nenhuma matéria encontrada"
                      hide-details
                    />
                  </v-col>
                  <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                    <v-text-field
                      label="Pesquise por dúvidas..."
                      dense
                      clearable
                      outlined
                      rounded
                      hide-details
                      color="#3b71fe" 
                      height="55px"
                      class="alinhamentoIcon"
                      v-model="filtro.pergunta"
                      @keypress.enter="listarDuvidas(false)"
                      @keyup="validaBotaoPesquisa"
                      @click:clear="botaoPesquisa = true"
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
                class="card-inferior-duvida-botao-fechar"
                @click="closeModalFiltro"
              >
                <v-icon small>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                class="card-inferior-duvida-botao"
                @click="listarDuvidas(false) && closeModalFiltro()"
              >
                <v-icon small>mdi-check</v-icon>
                Filtrar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
      <v-row justify="center">
        <v-dialog persistent content-class="rounded-xl" v-model="modalResponderDuvida" max-width="1200px">
          <v-card rounded="xl">
            <v-toolbar dark color="#3b71fe">
              <v-toolbar-title>Dúvida</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-btn icon dark @click="closeModalResponderDuvida">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </v-toolbar>
            <v-card-text>
              <v-form ref="formResposta" class="pt-4">
                <v-row class="pt-4 pb-4">
                  <v-card elevation="0" shaped width="100%">
                    <v-col cols="12" xs="12" class="pt-2 pl-8 pr-0 pb-2">
                      <h1 style="color:  rgba(0, 0, 0, 0.6);">{{duvida.pergunta ? duvida.pergunta : ''}}</h1>
                    </v-col>
                    <v-card-actions class="pt-0" v-if="duvida.usuarioId == usuario.id">
                      <v-spacer></v-spacer>
                      <v-btn
                        v-if="!validaIconeMelhorResposta(duvida.respostas)"
                        color="#3b71fe"
                        text
                        rounded
                        outlined
                        class="card-inferior-duvida-botao"
                        @click="selecionarItem(duvida)"
                        >
                          <v-icon small>mdi-pencil-outline</v-icon>
                          Editar Pergunta
                      </v-btn>
                    </v-card-actions>
                  </v-card>
                  <v-card elevation="0" rounded="xl" :class="(duvida.usuarioId == usuario.id ? '' : 'mt-4 ') + 'card-interno-resposta'">
                    <v-list class="card-resposta-lista">

                      <v-card
                        class="pt-4 mt-0 ml-4 mb-4 card-respota"
                        rounded="xl"
                        elevation="0"
                        color="#DEDEE0"
                        v-if="!validaBotaoCardDuvida(duvida.respostas) && duvida.usuarioId != usuario.id"
                      >
                        <v-textarea
                          label="Resposta"
                          v-model="resposta"
                          dense
                          clearable
                          outlined
                          rounded
                          maxlength="4000"
                          counter="4000"
                          no-resize
                          height="100px"
                          color="#3b71fe"
                          background-color="white"
                          class="alinhamentoIcon"
                          :rules="[(v) => !!v || 'O campo resposta é obrigatório']"
                        />
                        <v-card-actions class="pt-0">
                          <v-spacer></v-spacer>
                          <v-btn
                            color="#3b71fe"
                            text
                            rounded
                            outlined
                            class="card-inferior-duvida-botao"
                            @click="responderDuvida"
                            >
                              <v-icon small>mdi-check</v-icon>
                              Salvar
                          </v-btn>
                        </v-card-actions>
                      </v-card>

                      <template v-for="(resposta, index) in duvida.respostas">
                        <v-list-tile :key="index" >
                          <v-list-tile-content>
                            <v-card 
                            :class="(index == 0 ? 'mt-0 ' : '') + 'ml-4 card-respota'"
                            rounded="xl"
                            elevation="0"
                            color="#DEDEE0"
                            >
                              <v-toolbar flat color="transparent">
                                <v-toolbar-title>
                                  <div class="card-respota-titulo">
                                    <v-icon size="30px" class="mr-2 icone-titulo">mdi-account-edit-outline</v-icon>
                                    <div class="mr-4">{{ resposta.nomeUsuario }}</div>
                                  </div>
                                </v-toolbar-title>
                                <v-spacer></v-spacer>
                                <v-icon
                                  v-if="!validaIconeMelhorResposta(duvida.respostas) && duvida.usuarioId == usuario.id"
                                  size="30px"
                                  class="mr-2 icone-titulo"
                                  color="#3b71fe"
                                  @click="modalMarcarMelhorResposta(resposta)"
                                >
                                  mdi-heart-outline
                                </v-icon>
                                <v-tooltip content-class="span-pontos" bottom v-if="validaMelhorResposta(resposta)">
                                  <template v-slot:activator="{ on }">
                                    <v-icon
                                      size="30px"
                                      class="mr-2 icone-titulo"
                                      color="red"
                                      v-on="on"
                                      x-large
                                    >
                                      mdi-heart
                                    </v-icon>
                                  </template>
                                  <span class="span">Melhor resposta</span>
                                </v-tooltip>
                                <v-rating
                                  class="d-none"
                                  v-if="duvida.usuarioId == usuario.id"
                                  empty-icon="mdi-heart-outline"
                                  full-icon="mdi-heart"
                                  hover
                                  dense
                                  :value="melhorResposta"
                                  @click="melhorResposta = 0"
                                  length="1"
                                  size="25"
                                  color="red"
                                >
                                </v-rating>
                              </v-toolbar>
                              <v-card-text>
                                <h2>{{ resposta.descricao }}</h2>
                              </v-card-text>
                              <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn
                                  color="red"
                                  text
                                  rounded
                                  outlined
                                  class="card-inferior-duvida-botao-fechar"
                                  @click="excluirResposta(resposta)"
                                  v-if="validaBotaoEditarResposta(resposta) && !validaIconeMelhorResposta(duvida.respostas)"
                                >
                                    <v-icon small>mdi-trash-can-outline</v-icon>
                                    Excluir
                                </v-btn>
                                <v-btn
                                  color="#3b71fe"
                                  text
                                  rounded
                                  outlined
                                  class="card-inferior-duvida-botao"
                                  @click="editarResposta(resposta)"
                                  v-if="validaBotaoEditarResposta(resposta) && !validaIconeMelhorResposta(duvida.respostas)"
                                >
                                    <v-icon small>mdi-pencil-outline</v-icon>
                                    Editar
                                </v-btn>
                              </v-card-actions>
                            </v-card>
                          </v-list-tile-content>
                        </v-list-tile>
                      </template>
                    </v-list>
                  </v-card>
                </v-row>
              </v-form>
            </v-card-text>
          </v-card>
        </v-dialog>
      </v-row>
      <v-row justify="center">
        <v-dialog persistent content-class="rounded-xl" v-model="modalEdicaoResposta" max-width="1100px" @keydown.esc="modalEdicaoResposta = false">
          <v-card rounded="xl" color="DEDEE0">
            <v-card-text style="background-color: #DEDEE0">
              <v-form ref="formResposta" class="pt-4">
                <v-row class="pt-4 pb-4">
                  <v-card
                    class="pt-4 mt-0 ml-4 mb-4 card-respota"
                    rounded="xl"
                    elevation="0"
                    color="#DEDEE0"
                  >
                    <v-textarea
                      label="Resposta"
                      v-model="resposta"
                      dense
                      clearable
                      outlined
                      rounded
                      maxlength="4000"
                      counter="4000"
                      auto-grow
                      no-resize
                      height="100px"
                      color="#3b71fe"
                      background-color="white"
                      class="alinhamentoIcon"
                      :rules="[(v) => !!v || 'O campo resposta é obrigatório']"
                    />
                    <v-card-actions class="pt-0">
                      <v-spacer></v-spacer>
                      <v-btn
                        color="red"
                        text
                        rounded
                        outlined
                        class="card-inferior-duvida-botao-fechar"
                        style="border-color: red"
                        @click="modalEdicaoResposta = false"
                        >
                          <v-icon small>mdi-close</v-icon>
                          Cancelar
                      </v-btn>
                      <v-btn
                        color="#3b71fe"
                        text
                        rounded
                        outlined
                        class="card-inferior-duvida-botao"
                        @click="responderDuvida"
                        >
                          <v-icon small>mdi-check</v-icon>
                          Salvar
                      </v-btn>
                    </v-card-actions>
                  </v-card>
                </v-row>
              </v-form>
            </v-card-text>
          </v-card>
        </v-dialog>
      </v-row>
      <v-row justify="center">
        <v-dialog persistent content-class="rounded-xl" v-model="modalMelhorResposta" max-width="1100px" @keydown.esc="modalMelhorResposta = false">
          <v-card rounded="xl">
            <v-toolbar flat color="#eead2d">
              <v-toolbar-title>
                <div class="card-respota-titulo">
                  <h2 class="mr-4" style="color: white">Melhor Resposta</h2>
                </div>
              </v-toolbar-title>
            </v-toolbar>
            <v-card-text>
              <v-form ref="formResposta" class="pt-4">
                <v-row class="pt-4 pb-4">
                  <div class="d-grid">
                    <h1> Deseja marcar como a melhor resposta para a sua dúvida ? </h1>
                    <h2 class="mt-4 ml-4"> Resposta : "{{resposta}}" </h2>
                  </div>
                </v-row>
              </v-form>
            </v-card-text>
            <v-card-actions class="pt-0">
              <p style="color: #ff5252"><b>Marcar como "melhor resposta" finalizará a dúvida e ela não podera receber mais respostas</b></p>
              <v-spacer></v-spacer>
              <v-btn
                color="#ff5252"
                text
                rounded
                outlined
                class="card-inferior-duvida-botao-fechar"
                @click="modalMelhorResposta = false"
                >
                  <v-icon small>mdi-close</v-icon>
                  Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                outlined
                class="card-inferior-duvida-botao"
                @click="marcarMelhorResposta"
                >
                  <v-icon small>mdi-check</v-icon>
                  Salvar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
    </div>
  </div>
</template>
<script>
import Header from "../components/header.vue";
import UsuarioService from "../service/usuario-service.js";
import DuvidaService from "../service/duvida-service.js";
import MateriaService from "../service/materia-service.js";
import RespostaService from "../service/resposta-service.js";
import Loading from "../components/loading.vue";
import ModalSucesso from "../components/modal-sucesso.vue";
import MensagemErro from "../components/mensagem-erro.vue";
import DatePicker from "../components/date-picker.vue";
import Vue from "vue";

const usuarioService = new UsuarioService;
const duvidaService = new DuvidaService;
const materiaService = new MateriaService;
const respostaService = new RespostaService;
const vue = new Vue();

export default {
  components: {
    Header,
    Loading,
    ModalSucesso,
    MensagemErro,
    DatePicker
  },
  data() {
    return {
      modalMelhorResposta: false,
      melhorResposta: 0,
      modalEdicaoResposta: false,
      respostaId: null,
      resposta: null,
      objetoResposta: null,
      duvida: { pergunta: null },
      modalResponderDuvida: false,
      menuDataInicio: false,
      modalFiltro: false,
      carregarMais: false,
      botaoPesquisa: true,
      mensagemAlerta: "",
      alertaSucesso: false,
      alertaErro: false,
      mensagemTitulo: "",
      loading: false,
      modalDadosMestre: false,
      itemCache: null,
      item: null,
      paginas: null,
      totalItens: null,
      usuario: {},
      cursosAtivos: [],
      duvidas: [],
      materias: [],
      defaultInsertObject: {
        id: null,
        pergunta: "",
        pontos: 25,
        dataHora: null,
        usuarioId: null,
        materiaId: "",
        quantidadeResposta: 0,
        ativo: true,
      },
      filtro: {
        listaMateria: [],
        usuarioId: null,
        perfilId: null,
        pergunta: null,
        dataInicio: null,
        dataFim: null,
        minhasDuvidas: false,
        pagina: 1,
        itensPagina: 10
      }
    };
  },
  methods: {
    async excluirItem(duvida) {
      this.loading = true;
      let result = await duvidaService.deletar(duvida.id)
      if (result?.statusCode === 200) {
        this.mensagemAlerta =
        "A dúvida foi excluida com sucesso";
        this.mensagemTitulo = "Excluir dúvida";
        this.modalEdicaoResposta = false;
          this.alertaSucesso = true;
          this.respostaId = null;
          this.resposta = null;
          await this.listarDuvidas();
          this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    validaExclusao(duvida) {
      const login = JSON.parse(localStorage.getItem("login"));
      if(duvida.usuarioId == login.id && duvida.respostas <= 0) {
        return true;
      } else {
        return false;
      }
    },
    validaPerfilAluno() {
      const login = JSON.parse(localStorage.getItem("login"));
      if(vue.$globals.perfilAluno == login.perfilId) {
        return true;
      } else {
        return false;
      }
    },
    validaMelhorResposta(resposta) {
      if(resposta.melhorResposta == true) {
        return true;
      } else {
        return false;
      }
    },
    async marcarMelhorResposta() {
      this.loading = true;
      const resposta = {
          id: this.objetoResposta.id,
          descricao: this.resposta,
          melhorResposta: true,
          usuarioId: this.objetoResposta.usuarioId,
          duvidaId: this.duvida.id
      }
      let result = await respostaService.atualizar(resposta);
      if (result?.statusCode === 200) {
        this.mensagemAlerta =
          "Melhor resposta marcada com sucesso. Os pontos foram atribuídos ao dono da resposta e a dúvida foi finalizada.";
        this.mensagemTitulo = "Melhor Resposta";
        this.modalMelhorResposta = false;
        this.alertaSucesso = true;
        this.$refs.formResposta.resetValidation();
        this.loading = false;
        this.respostaId = null;
        this.resposta = null;
        await this.listarDuvidas();
        this.objetoResposta.melhorResposta = true;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    modalMarcarMelhorResposta(resposta) {
      this.objetoResposta = resposta;
      this.resposta = resposta.descricao;
      this.modalMelhorResposta = true;
    },
    validaIconeMelhorResposta(respostas) {
      if(respostas?.length > 0) {
        const respostaFavorita = respostas.filter(x => x.melhorResposta === true);
        if(respostaFavorita.length > 0) {
          return true;
        } else {
          return false;
        }
      } else {
        return false;
      }
    },
    async excluirResposta(resposta) {
      this.loading = true;
      let result = await respostaService.deletar(resposta.id)
      if (result?.statusCode === 200) {
        this.mensagemAlerta =
          "A resposta foi excluida com sucesso";
        this.mensagemTitulo = "Excluir resposta";
        this.modalEdicaoResposta = false;
        this.alertaSucesso = true;
        this.$refs.formResposta.resetValidation();
        this.loading = false;
        this.respostaId = null;
        this.resposta = null;
        await this.listarDuvidas();
        let result = await respostaService.listarRespostas(this.duvida.id);
        this.duvida.respostas = result?.data;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    editarResposta(resposta) {
      this.respostaId = resposta.id;
      this.resposta = resposta.descricao;
      this.modalEdicaoResposta = true;
    },
    validaBotaoEditarResposta(resposta) {
      if(resposta.usuarioId == this.usuario.id) {
        return true;
      } else {
        return false;
      }
    },
    async responderDuvida() {
      if (this.$refs.formResposta.validate()) {
        const resposta = {
          id: this.respostaId,
          descricao: this.resposta,
          melhorResposta: false,
          usuarioId: this.usuario.id,
          duvidaId: this.duvida.id
        }

        this.loading = true;

        let result = resposta.id != null
          ? await respostaService.atualizar(resposta)
          : await respostaService.cadastrar(resposta);
        if (result?.statusCode === 200) {
          if (resposta.id != null) {
            this.mensagemAlerta =
              "A resposta foi editada com sucesso";
            this.mensagemTitulo = "Edição da resposta";
          } else {
            this.mensagemAlerta =
              "A resposta foi inserida com sucesso";
            this.mensagemTitulo = "Inserção da resposta";
          }
          this.modalEdicaoResposta = false;
          this.alertaSucesso = true;
          this.$refs.formResposta.resetValidation();
          this.loading = false;
          this.respostaId = null;
          this.resposta = null;
          await this.listarDuvidas();
          let result = await respostaService.listarRespostas(this.duvida.id);
          this.duvida.respostas = result?.data;
        } else {
          this.mensagemAlerta = result?.data.mensagem;
          this.alertaErro = true;
          this.loading = false;
        }
      }
    },
    validaBotaoCardDuvida(respostas) {
      if(respostas?.length > 0) {
        const respondidoPeloUsuarioLogado = respostas.filter(x => x.usuarioId === this.usuario.id);
        const respostaFavorita = respostas.filter(x => x.melhorResposta === true);
        if(respondidoPeloUsuarioLogado?.length > 0 || respostaFavorita.length >= 1) {
          return true;
        } else {
          return false;
        }
      } else {
        return false;
      }
    },
    botaoResponderCard(duvida) {
      this.duvida = duvida;
      this.modalResponderDuvida = true;
    },
    closeModalResponderDuvida() {
      this.$refs.formResposta.resetValidation();
      this.respostaId = null;
      this.resposta = null;
      this.duvida = { pergunta: null };
      this.modalResponderDuvida = false;
    },
    selecionarData(event){
      this.filtro[event.model] = event.data;
    },
    closeModalFiltro() {
      this.modalFiltro = false;
    },
    async carregarMaisDuvidas() {
      await this.listarDuvidas(true);
    },
    selecionarItem(item){
      this.itemCache = item;
      this.item = JSON.parse(JSON.stringify(item));
      this.modalDadosMestre = true;
    },
    async salvarItem() {
      this.validado = true;
      if (this.$refs.formDadosMestre.validate()) {
        this.loading = true;
        this.item.usuarioId = this.usuario.id;
        let result = this.item.id
          ? await duvidaService.atualizar(this.item)
          : await duvidaService.cadastrar(this.item);
        if (result?.statusCode === 200) {
          if (this.item.id) {
            this.mensagemAlerta =
              "A dúvida '" + this.item.pergunta + "' foi editado com sucesso";
            this.mensagemTitulo = "Edição de dúvida";
            this.duvida.pergunta = this.item.pergunta;
          } else {
            this.mensagemAlerta =
              "A dúvida '" + this.item.pergunta + "' foi inserido com sucesso";
            this.mensagemTitulo = "Inserção de dúvida";
          }
          this.alertaErro = false;
          this.alertaSucesso = true;
          this.closeModalDadosMestre();
          this.$refs.formDadosMestre.resetValidation();
          await this.listarDuvidas(false);

          this.defaultInsertObject.pergunta = "";
          this.defaultInsertObject.materiaId = "";
        } else {
          this.mensagemAlerta = result?.data.mensagem;
          this.alertaErro = true;
          this.loading = false;
        }
      }
    },
    clickInserirDuvida() {
      this.item = this.defaultInsertObject;
      this.modalDadosMestre = true;
    },
    closeModalDadosMestre() {
      this.itemCache = null;
      this.item = null;
      this.modalDadosMestre = false
    },
    validaBotaoPesquisa() {
      if(this.filtro.pergunta && this.filtro.pergunta?.length > 0) {
        this.botaoPesquisa = false;
      } else {
        this.botaoPesquisa = true;
      }
    },
    handleFocusOut() {
      this.botaoPesquisa = true;
    },
    async listarMaterias() {
      this.loading = true;
      const login = JSON.parse(localStorage.getItem("login"));
      let result = await materiaService.GetAllByUsuarioId(login.id);
      if (result?.statusCode === 200) {
        this.materias = result?.data;
        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    async listarDuvidas(carregarMais) {
      this.loading = true;
      const login = JSON.parse(localStorage.getItem("login"));
      this.filtro.usuarioId = login.id;
      this.filtro.perfilId = login.perfilId;
      let result = await duvidaService.listarDuvidas(this.filtro);
      if (result?.statusCode === 200) {
        if(!carregarMais) {
          this.filtro.pagina = 1;
          this.duvidas = result?.data.lista;
        } else {

          if(this.filtro.pagina < this.paginas){
            this.filtro.pagina = this.filtro.pagina + 1;
          }

          this.duvidas = this.duvidas.concat(result?.data.lista);
        }

        this.paginas = result?.data.paginas;
        this.totalItens = result.data.totalItens;
        
        if(this.filtro.pagina < result?.data.paginas){
          this.carregarMais = true;
        } else {
          this.carregarMais = false;
        }

        this.loading = false;
      } else {
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    },
    async minhasDuvidas() {
      this.filtro.minhasDuvidas = !this.filtro.minhasDuvidas;
      await this.listarDuvidas();
    },
    async carregarUsuario() {
      const login = JSON.parse(localStorage.getItem("login"));
      const result = await usuarioService.getById(login.id);
      if (result?.statusCode === 200) {
        this.usuario = result?.data;
        this.usuario.nome = this.usuario.nome.split(' ')[0];
        this.cursosAtivos = this.usuario.cursos.filter(x => x.ativo === true);
        this.loading = false;
      } else {
        this.usuario.nome = login.nome.split(' ')[0];
        this.mensagemAlerta = result?.data.mensagem;
        this.alertaErro = true;
        this.loading = false;
      }
    }
  },
  created: function() {
    this.carregarUsuario();
    this.listarDuvidas(false);
    this.listarMaterias();
  },
};
</script>
<style scoped>
.app-container {
  display: flex;
}
.container-esquerdo {
  width: 15%;
  height: 100%;
}
.container-direito {
  width: 85%;
  height: 100%;
}
.card-superior {
  margin-left: 25px;
  justify-content: left;
  align-items: center;
  display: flex;
  width: 98%;
  height: 100px;
}
.card-superior-botao {
  border: #3b71fe solid 2px !important;
  margin: 0 0 0 30px;
}
.card-inferior {
  margin: 25px 0px 0px 25px ;
  padding: 5px 0 5px 0;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 98%;
  height: 675px;
  border: #3b71fe solid 1px !important;
}
.card-inferior-lista {
  width: 97%;
  max-height: 97%;
  flex-grow: 1; 
  overflow: auto;
  background: transparent;
  box-shadow: 0;
  justify-items: start;
}
.card-inferior-duvida {
  margin: 20px 6px 0px 12px;
  height: auto;
  width: calc(100% - 40px);
  justify-content: left;
  align-items: left;
}
.card-inferior-duvida-titulo {
  font-family: 'Poppins' !important;
  font-weight: bold;
  font-size: 30px;
  color:  rgba(0, 0, 0, 0.6);
  display: flex; 
  align-items: center;
}
.card-inferior-duvida-titulo-pontos {
  border: 2px solid #3b71fe;
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background-color: #3b71fe;
  color: white;
  display: flex;
  justify-content: center;
  align-items: center;
  font-weight: 600;
}
.span-pontos {
  border-radius: 20px;
}
.card-inferior-duvida-botao {
  border: #3b71fe solid 2px !important;
  caret-color: #3b71fe;
}
.card-inferior-duvida-botao-fechar {
  border: #ff5252 solid 2px !important;
  caret-color: #ff5252;
}
.card-respota {
  margin: 20px 6px 0px 12px;
  height: auto;
  width: calc(100% - 40px);
  justify-content: left;
  align-items: left;
}
.card-respota-titulo {
  font-family: 'Poppins' !important;
  font-weight: bold;
  font-size: 20px;
  color:  rgba(0, 0, 0, 0.6);
  display: flex; 
  align-items: center;
}
.card-interno-resposta {
  margin: 0px 25px 25px 25px ;
  padding: 5px 0 5px 0;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 98%;
  max-height: 675px;
  border: #3b71fe solid 1px !important;
}
.card-resposta-lista {
  width: 97%;
  flex-grow: 1; 
  overflow: auto;
  background: transparent;
  box-shadow: 0;
  justify-items: start;
}
.card-lateral {
  width: 100%;
  height: 800px;
}
.card-lateral-info {
  display: grid;
  justify-content: center;
  font-size: 20px;
  width: 100%;
}
.card-lateral-titulo{
  display: flex;
  justify-content: left;
  text-align: center;
  margin-bottom: 20px;
}
.card-lateral-titulo p{
  display: flex;
  align-items: center;
  margin-bottom: 0;
  font-size: 25px;
}
.card-lateral-subtitulo{
  display: grid;
  justify-items: left;
  width: 100%;
  margin-bottom: 0;
  font-size: 16px;
  font-weight: 600;
  color: #71717A;
}
.card-lateral-subtitulo p{
  margin-bottom: 0px !important;
}
.card-lateral-icon {
  display: flex;
  margin-right: 10px;
  color: red;
}
.card-lateral-pontos {
  font-size: 30px;
  font-family: 'Poppins';
  font-weight: 900;
  margin-top: 15px;
  display: flex;
  justify-content: left;
}
.card-lateral-botao {
  border: #3b71fe solid 2px !important;
  caret-color: #3b71fe;
}
.card-insert {
  border-radius: 0.5em;
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
.response {
  display: none;
}
.v-card--link:focus:before {
  opacity: 0;
}
.button-dark {
  background-color: #3b71fe !important;
  color: white !important;
}
.aviso-zero-duvidas {
  display: flex; 
  justify-content: center;
  align-items: center;
}

@media (max-width: 1610px) {
  .container-esquerdo {
    width: 20%;
    transition: all 1s;
  }
  .container-direito {
    width: 80%;
    transition: all 1s;
  }
}

@media (max-width: 1140px) {
  .container-esquerdo {
    display: none;
    width: 30%;
    transition: all 1s;
  }
  .response {
    display: flex;
  }
  .container-direito {
    width: 100%;
    transition: all 1s;
  }
  .card-superior {
    margin-left: 0;
    width: 100%;
    transition: all 1s;
  }
  .card-inferior {
    margin: 25px 0px 0px 0 ;
    width: 100%;
    padding: 3px 0 3px 0;
    transition: all 1s;
  }
}
</style>