import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";

const vue = new Vue();

class DuvidaService extends StorageService {
    async listarEventos(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "evento/listarEventos", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async cadastrar(evento) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(evento)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "evento", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async atualizar(evento) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'PUT',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(evento)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "evento/" + evento.id, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }

    async deletar(eventoId) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'DELETE',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "evento/" + eventoId, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
}

export default DuvidaService;