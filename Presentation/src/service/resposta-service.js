import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";

const vue = new Vue();

class RespostaService extends StorageService {
    async listarRespostas(duvidaId) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "resposta/duvida/" + duvidaId, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async cadastrar(resposta) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(resposta)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "resposta", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async atualizar(resposta) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'PUT',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(resposta)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "resposta/" + resposta.id, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async deletar(respostaId) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'DELETE',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "resposta/" + respostaId, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
}

export default RespostaService;