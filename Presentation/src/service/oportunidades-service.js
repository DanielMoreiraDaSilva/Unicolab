import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";

const vue = new Vue();

class OportunidadeService extends StorageService {
    async GetAllOportunidades() {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "oportunidade", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async listarOportunidades(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "oportunidade/listarOportunidades", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async cadastrar(oportunidade) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(oportunidade)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "oportunidade", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async atualizar(oportunidade) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'PUT',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(oportunidade)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "oportunidade/" + oportunidade.id, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async deletar(oportunidadeId) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'DELETE',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "oportunidade/" + oportunidadeId, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async listarCampos() {
        return [
            {
                descricao: "Título",
                valor: "titulo",
                selecionado: null,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Status",
                valor: "statusFormatado",
                selecionado: null,
                editavel: false,
                filtravel: true,
                tipo: "lista",
                lista: [
                    { value: null, text: 'Ambos' },
                    { value: true, text: 'Ativos' },
                    { value: false, text: 'Inativos' },
                ],
                itemText: "text",
                itemValue: "value",
                valorFiltroEmLista: "ativos",
                modelFiltro: null,
                modelUltimoValor: null,
                filtrado: false,
                ordenado: null,
            },
        ]
    }

    listarFiltros() {
        return {
            todos: null,
            titulo: null,
            ativos: null,
            ordenarPor: 0,
            ordem: 0,
            pagina: 1,
            itensPagina: 10,
        };
    }
}

export default OportunidadeService;