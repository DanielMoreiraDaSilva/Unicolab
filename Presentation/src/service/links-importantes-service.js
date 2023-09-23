import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";

const vue = new Vue();

class LinkImportanteService extends StorageService {
    async listarLinksImportantes() {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "linkImportante", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async listarGridLinksImportantes(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "linkImportante/listarLinks", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async cadastrar(linkImportante) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(linkImportante)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "linkImportante", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async atualizar(linkImportante) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'PUT',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(linkImportante)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "linkImportante/" + linkImportante.id, requestOptions);
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
                descricao: "Link",
                valor: "url",
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
            url: null,
            ativos: null,
            ordenarPor: 0,
            ordem: 0,
            pagina: 1,
            itensPagina: 10,
        };
    }
}

export default LinkImportanteService;