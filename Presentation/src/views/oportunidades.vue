<template>
    <div>
        <Loading v-if="loading" />
        <Header titulo="Oportunidades" homeModulo="/modulos" tooltipModulo="Home"/>
        <div class="pa-6 mb-0 app-container">
            <v-card
                height="100px"
                rounded="xl"
                elevation="4"
                class="card-superior"
                color="#F4F4F5">
                <v-btn
                    rounded="rounded"
                    outlined="outlined"
                    color="#3b71fe"
                    text="text"
                    x-large="x-large"
                    elevation="2"
                    class="card-superior-botao"
                    @click="clickInserirOportunidade">
                    <v-icon large="large" left="left">mdi-plus</v-icon>
                    Oportunidade
                </v-btn>
                <v-btn
                    rounded
                    outlined
                    color="#3b71fe"
                    text
                    x-large
                    elevation="2"
                    :class="'card-superior-botao responsive' + (filtro.criadasPorMim == true ? ' button-dark' : '')"
                    @click="CriadasPorMim">
                    <v-icon large="large" left="left">mdi-account</v-icon>
                    Criadas por mim
                </v-btn>
                <v-btn
                    rounded="rounded"
                    outlined="outlined"
                    color="#3b71fe"
                    text="text"
                    x-large="x-large"
                    elevation="2"
                    class="card-superior-botao"
                    @click="modalFiltro = true">
                    <v-icon large="large" left="left">mdi-filter-outline</v-icon>
                    Filtrar
                </v-btn>
            </v-card>
            <v-card rounded="xl" elevation="4" class="card-inferior" color="#F4F4F5">
                <v-row class="lista-cards" v-if="oportunidades.length > 0">
                    <v-col
                        cols="12"
                        xl="2"
                        lg="3"
                        md="4"
                        sm="5"
                        xs="6"
                        v-for="(oportunidade) in oportunidades"
                        :key="oportunidade.id"
                        style="display: flex; justify-content: center;">
                        <v-card
                            class="card-oportunidade"
                            rounded="xl"
                            elevation="4"
                            color="#DEDEE0"
                            width="100%"
                            height="230px"
                        >
                            <v-toolbar flat color="transparent">
                                <v-toolbar-title>
                                    <div >
                                        <div class="titulo">{{ oportunidade.titulo }}</div>
                                    </div>
                                </v-toolbar-title>
                                <v-spacer></v-spacer>
                            </v-toolbar>
                            <v-card-text class="card-oportunidade-text" @click="selecionarItem(oportunidade)" style="cursor: pointer">
                                <h3 class="info-cards">{{ oportunidade.cursoDescricao }}</h3>
                                <h3 class="info-cards">{{ oportunidade.empresa }}</h3>
                                <h3 class="info-cards">Data: {{ oportunidade.dataInicioFormatada }}</h3>
                                <h3 class="info-cards">Salário: {{ oportunidade.salarioFormatado }}</h3>
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn
                                    color="red"
                                    rounded
                                    outlined
                                    fab
                                    x-small
                                    class="mr-1 botao-fechar"
                                    @click="excluirItem(oportunidade)"
                                    v-if="validaEdicao(oportunidade)"
                                >
                                    <v-icon >mdi-trash-can-outline</v-icon>
                                </v-btn>
                                <v-btn
                                    color="#3b71fe"
                                    rounded
                                    outlined
                                    fab
                                    x-small
                                    class="ml-2"
                                    @click="editarItem(oportunidade)"
                                    v-if="validaEdicao(oportunidade)"
                                >
                                    <v-icon >mdi-pencil-outline</v-icon>
                                </v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-col>
                </v-row>
                <v-row v-else>
                    <v-col class="aviso-zero-oportunidades">
                        <h4 style="color: red; font-family: Poppins">
                            NÃO EXISTEM OPORTUNIDADES DISPONÍVEIS NO MOMENTO
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
        <ModalErro
            :mensagem="mensagemAlerta"
            :alertaErro="alertaErro"
            @ocultarErro="ocultarErro"/>

        <v-row justify="center" v-if="item">
            <v-dialog
                persistent="persistent"
                content-class="rounded-xl"
                v-model="modalDadosMestre"
                fullscreen>
                <v-card rounded="xl">
                    <v-toolbar dark="dark" color="#3b71fe">
                        <v-toolbar-title>{{item.id ? "Editar Oportunidade" : "Inserir Oportunidade"}}
                        </v-toolbar-title>
                        <v-spacer></v-spacer>
                        <v-btn icon="icon" dark="dark" @click="closeModalDadosMestre">
                            <v-icon>mdi-close</v-icon>
                        </v-btn>
                    </v-toolbar>
                    <v-card-text>
                        <v-form ref="formDadosMestre" class="pt-4">
                            <v-row class="pt-2 pb-4">
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
                                    <v-text-field
                                        label="Empresa"
                                        v-model="item.empresa"
                                        dense="dense"
                                        clearable="clearable"
                                        outlined="outlined"
                                        rounded="rounded"
                                        height="60px"
                                        color="#3b71fe"
                                        class="alinhamentoIcon"
                                        :rules="[(v) => !!v || 'O campo empresa é obrigatório']"/>
                                </v-col>
                                <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                                    <DatePicker
                                        @selecionarData="selecionarDataFim($event)"
                                        :obrigatorio="false"
                                        model="dataFim"
                                        :retornarHorario="false"
                                        :horarioInicial="true"
                                        label="Data Fim"
                                        v-model="item.dataFim"
                                        :ocultarDetalhes="false"
                                        dense="dense"
                                        clearable="clearable"
                                        outlined="outlined"
                                        rounded="rounded"
                                        color="#3b71fe"
                                        :rules="[(v) => !!v || 'O campo data é obrigatório']"
                                    />
                                </v-col>
                                <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
                                    <v-text-field
                                        label="Salário"
                                        v-model="item.salario"
                                        dense
                                        clearable="clearable"
                                        outlined
                                        rounded
                                        height="60px"
                                        color="#3b71fe"
                                        class="alinhamentoIcon"
                                        style="font-family: Poppins"
                                        prefix="R$"
                                        v-mask="[
                                            '#,##',
                                            '##,##',
                                            '###,##',
                                            '####,##',
                                            '#####,##',
                                            '######,##',
                                            '#######,##',
                                            '########,##',
                                            '#########,##',
                                            '##########,##',
                                            '###########,##',
                                            '############,##',
                                            '#############,##',
                                            '##############,##',
                                            '###############,##',
                                            '################,##',

                                        ]"
                                        :rules="[(v) => !!v || 'O campo salário é obrigatório']"/>
                                </v-col>
                                <v-col cols="12" xs="12" class="pt-2 pl-0 pr-0 pb-0">
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
                                        height="300"
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
                <v-btn icon dark @click="closeModalFiltro">
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
                @click="closeModalFiltro"
              >
                <v-icon small>mdi-close</v-icon>Cancelar
              </v-btn>
              <v-btn
                color="#3b71fe"
                text
                rounded
                @click="listarOportunidades(false) && closeModalFiltro()"
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
                persistent="persistent"
                content-class="rounded-xl"
                v-model="modalDetalhesOportunidade"
                width="1000px"
            >
                <v-card rounded="xl">
                    <v-toolbar dark="dark" color="#3b71fe">
                        <v-toolbar-title style="font-family: 'Poppins'; font-size: 30px; font-weight: 600">
                            {{item.cursoDescricao}}
                        </v-toolbar-title>
                        <v-spacer></v-spacer>
                        <v-btn icon="icon" dark="dark" @click="modalDetalhesOportunidade = false">
                            <v-icon>mdi-close</v-icon>
                        </v-btn>
                    </v-toolbar>
                    <v-card-text class="card-oportunidade-detalhe">
                        <div style="font-family: 'Poppins'; font-size: 16px"><h1 class="mb-8">{{item.titulo}}</h1></div>
                        <h2 class="mb-6">Espresa : {{item.empresa}}</h2>
                        <h2 class="mb-6">Data : {{item.dataInicioFormatada}}</h2>
                        <h2 class="mb-6">Salário : {{item.salarioFormatado}}</h2>
                        <h2 class="mb-6">Autor : {{item.usuarioNome}}</h2>
                        <h2 class="mb-6">Descricação : {{item.descricao}}</h2>
                    </v-card-text>
                </v-card>
            </v-dialog>
        </v-row>
    </div>
</template>
<script>
    import { mask } from "vue-the-mask";
    import Header from "../components/header.vue";
    import OportunidadeService from "../service/oportunidades-service.js"
    import CursoService from "../service/curso-service.js";
    import ModalErro from "../components/mensagem-erro.vue";
    import Loading from "../components/loading.vue";
    import ModalSucesso from "../components/modal-sucesso.vue";
    import DatePicker from "../components/date-picker.vue";
    import Paginacao from "../components/paginacao.vue";

    const oportunidadeService = new OportunidadeService();
    const cursoService = new CursoService;

    export default {
        directives: {
            mask,
        },
        components: {
            Header,
            Loading,
            ModalErro,
            ModalSucesso,
            DatePicker,
            Paginacao
        },
        data() {
            return {
                modalDetalhesOportunidade: false,
                totalPages: 1,
                totalItems: 1,
                menuDataInicio: false,
                carregarMais: false,
                botaoPesquisa: true,
                mensagemAlerta: "",
                alertaSucesso: false,
                alertaErro: false,
                mensagemTitulo: "",
                loading: false,
                modalDadosMestre: false,
                modalFiltro: false,
                paginas: [],
                oportunidades: [],
                cursos: [],
                item: {},
                defaultInsertObject: {
                    id: null,
                    cursoId: null,
                    usuarioId: null,
                    titulo: null,
                    empresa: null,
                    descricao: null,
                    salario: 0,
                    dataInicio: null,
                    dataFim: null,
                    ativo: true
                },
                filtro: {
                    todos: null,
                    titulo: null,
                    statusFormatado: null,
                    ativo: null,
                    listaCurso: [],
                    usuarioId: null,
                    criadasPorMim: false,
                    pagina: 1,
                    itensPagina: 12
                }
            };
        },
        methods: {
            async excluirItem(evento) {
                this.loading = true;
                let result = await oportunidadeService.deletar(evento.id)
                if (result?.statusCode === 200) {
                    this.mensagemAlerta =
                    "A oportunidade foi excluida com sucesso";
                    this.mensagemTitulo = "Excluir oportunidade";
                    this.alertaErro = false;
                    this.alertaSucesso = true;
                    this.$refs.formDadosMestre.resetValidation();
                    await this.listarOportunidades(false);
                    this.loading = false;
                } else {
                    this.mensagemAlerta = result?.data.mensagem;
                    this.alertaErro = true;
                    this.loading = false;
                }
            },
            async CriadasPorMim() {
                this.filtro.criadasPorMim = !this.filtro.criadasPorMim;
                await this.listarOportunidades();
            },
            validaEdicao(item) {
                const login = JSON.parse(localStorage.getItem("login"));
                if(item.usuarioId === login.id) {
                    return true;
                } else {
                    return false;
                }
            },
            alterarPagina(pagina) {
                this.filtro.pagina = pagina;
                this.listarOportunidades();
            },
            alterarItensPorPagina(itens) {
                this.filtro.itensPagina = itens;
                this.filtro.pagina = 1;
                this.listarOportunidades();
            },
            selecionarDataInicio(event){
                this.item[event?.model] = event?.data;
            },
            selecionarDataFim(event){
                this.item[event?.model] = event?.data;
            },
            selecionarData(event){
                this.filtro[event?.model] = event?.data;
            },
            closeModalFiltro() {
                this.modalFiltro = false;
            },
            ocultarErro() {
                this.mensagemAlerta = null;
                this.alertaErro = false;
            },
            validaBotaoPesquisa() {
                if(this.filtro.titulo && this.filtro.titulo?.length > 0) {
                    this.botaoPesquisa = false;
                } else {
                    this.botaoPesquisa = true;
                }
            },    
            handleFocusOut() {
                this.botaoPesquisa = true;
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
            async listarOportunidades() {
                this.loading = true;
                const login = JSON.parse(localStorage.getItem("login"));
                this.filtro.usuarioId = login.id;
                let listaCursoVazia = false;
                if(this.filtro.listaCurso.length === 0) {
                    listaCursoVazia = true;
                    this.filtro.listaCurso = login.listaCursoId;
                }
                let result = await oportunidadeService.listarOportunidades(this.filtro);
                if (result?.statusCode === 200) {
                    this.oportunidades = result?.data.lista;
                    this.totalPages = result?.data.paginas;
                    this.totalItems = result?.data.totalItens;
                    this.loading = false;
                    if(listaCursoVazia === true) {
                        this.filtro.listaCurso = []
                    }
                } else {
                    this.loading = false;
                    this.mensagemAlerta = result ?.data.mensagem;
                    this.alertaErro = true;
                }
            },
            editarItem(item){
                this.item = JSON.parse(JSON.stringify(item));
                this.modalDadosMestre = true;
            },
            selecionarItem(item){
                this.item = JSON.parse(JSON.stringify(item));
                this.modalDetalhesOportunidade = true;
            },
            async salvarItem() {
                this.loading = true;
                this.validado = true;
                if (this.$refs.formDadosMestre.validate()) {
                    this.loading = true;
                    const login = JSON.parse(localStorage.getItem("login"));
                    this.item.usuarioId = login.id;
                    this.item.salario = parseFloat(this.item.salario.toString().replace(",", "."));
                    let result = this.item.id
                    ? await oportunidadeService.atualizar(this.item)
                    : await oportunidadeService.cadastrar(this.item);
                    if (result?.statusCode === 200) {
                    if (this.item.id) {
                        this.mensagemAlerta =
                        "A oportunidade '" + this.item.titulo + "' foi editada com sucesso";
                        this.mensagemTitulo = "Edição de oportunidade";
                    } else {
                        this.mensagemAlerta =
                        "A oportunidade '" + this.item.titulo + "' foi inserida com sucesso";
                        this.mensagemTitulo = "Inserção de oportunidade";
                    }
                    this.alertaErro = false;
                    this.alertaSucesso = true;
                    this.closeModalDadosMestre();
                    this.$refs.formDadosMestre.resetValidation();
                    await this.listarOportunidades(false);
                    this.loading = false;
                    } else {
                    this.mensagemAlerta = result?.data.mensagem;
                    this.alertaErro = true;
                    this.loading = false;
                    }
                }
            },
            clickInserirOportunidade() {
                this.item = JSON.parse(JSON.stringify(this.defaultInsertObject));
                this.modalDadosMestre = true;
            },
            closeModalDadosMestre() {
                this.item = JSON.parse(JSON.stringify(this.defaultInsertObject));
                this.$refs.formDadosMestre.resetValidation();
                this.modalDadosMestre = false
            }
        },
        created: function () {
            this.listarOportunidades();
            this.listarCursos();
        }
    };
</script>
<style scoped="scoped">
    .card-oportunidade {
        font-size: 60px !important;
    }
    .card-superior {
        display: flex;
        align-items: center;
        margin-bottom: 30px;
    }
    .card-superior-botao {
        margin-left: 20px;
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
    .icone-oportunidade {
        margin-left: 20px;
        margin-right: 20px;
        color: #3B71FE;
    }
    .titulo-card {
        color: #26262A;
    }
    .info-cards {
        color: #71717A;
    }
    .titulo {
        color: #26262A;
        font-family: Poppins;
        font-weight: 600px;
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
    .card-inferior-colunas {
        min-width: 20% !important;
        display: grid;
        align-content: center;
        justify-content: left;
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
    .lista-cards {
        justify-content: left;
    }
    .paginacao {
        width: 100%;
    }
    .card-oportunidade-detalhe {
        margin-top: 30px;
    }
    .card-oportunidade-text:hover {
        transform: translateY(-1px);
        background-color: #c4c4c5;
    }
    .aviso-zero-oportunidades {
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

@media (max-width: 720px) {
    .responsive {
        display: none;
    }
}
</style>