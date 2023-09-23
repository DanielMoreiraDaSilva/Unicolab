import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";

const vue = new Vue();

class CursoService extends StorageService {
    async listarCursos(usuarioId) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "curso/" + usuarioId, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
}

export default CursoService;