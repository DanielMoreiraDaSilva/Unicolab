import Vue from "vue";
import Response from "./response.js";
import StorageService from "./storage-service.js";
import PerfilService from "./perfil-service.js";

const vue = new Vue();
const perfilService = new PerfilService();

class UsuarioService extends StorageService {
    listarFiltros() {
        return {
            todos: null,
            nome: null,
            login: null,
            email: null,
            perfil: null,
            statusFormatado: null,
            perfisIds: [],
            ativos: null,
            ordenarPor: 0,
            ordem: 0,
            pagina: 1,
            itensPagina: 10,
        };
    }

    listarFiltrosContatos() {
        return {
            todos: null,
            nome: null,
            login: null,
            email: null,
            statusFormatado: null,
            perfisIds: [vue.$globals.perfilAdmin, vue.$globals.perfilCoordenador, vue.$globals.perfilProfessor],
            ativos: true,
            ordenarPor: 0,
            ordem: 0,
            pagina: 1,
            itensPagina: 10,
        };
    }

    async listarCamposContatos() {
        const response = await perfilService.listarDescricoes();
        const perfilDescricoes = response.data.filter(x => x.id != vue.$globals.perfilAluno);
        return [
            {
                descricao: "Usuário",
                valor: "nome",
                selecionado: false,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Login",
                valor: "login",
                selecionado: false,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Perfil",
                valor: "perfilDescricao",
                selecionado: null,
                editavel: false,
                filtravel: true,
                tipo: "listaComposta",
                lista: perfilDescricoes,
                itemText: "descricao",
                itemValue: "id",
                valorFiltroEmLista: "perfisIds",
                modelFiltro: null,
                modelUltimoValor: null,
                filtrado: false,
                ordenado: null,
            },
            {
                descricao: "E-mail",
                valor: "email",
                selecionado: false,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Status",
                valor: "statusFormatado",
                selecionado: false,
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

    async listarCampos() {
        const perfilDescricoes = await perfilService.listarDescricoes();
        return [
            {
                descricao: "Reset",
                valor: "mdi-lock-open-outline",
                selecionado: null,
                editavel: false,
                filtravel: false,
                tipo: "botao",
                ordenado: null,
            },
            {
                descricao: "Usuário",
                valor: "nome",
                selecionado: null,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Login",
                valor: "login",
                selecionado: null,
                editavel: false,
                filtravel: false,
                tipo: "texto",
                ordenado: null,
            },
            {
                descricao: "Perfil",
                valor: "perfilDescricao",
                selecionado: null,
                editavel: false,
                filtravel: true,
                tipo: "listaComposta",
                lista: perfilDescricoes.data,
                itemText: "descricao",
                itemValue: "id",
                valorFiltroEmLista: "perfisIds",
                modelFiltro: null,
                modelUltimoValor: null,
                filtrado: false,
                ordenado: null,
            },
            {
                descricao: "E-mail",
                valor: "email",
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

    async listarUsuarios(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/listarUsuarios", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async listarUsuariosContatos(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/listarUsuariosContatos", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async listarUsuariosAtivos() {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/listarUsuariosAtivos", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async exportarUsuariosExcel(filtro) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(filtro),
                responseType: "blob"
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/usuariosExcel", requestOptions);
            const data = await fetchResponse.blob();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async cadastrar(usuario) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'POST',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(usuario)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async atualizar(usuario) {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'PUT',
                headers: vue.$globals.headerPadrao,
                body: JSON.stringify(usuario)
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/" + usuario.id, requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async efetuarLogin(usuario, senha) {
        const bodyRequest = {
            login: usuario, 
            senha: senha
        }
        const requestOptions = {
            method: 'POST',
            headers: vue.$globals.headerPadrao,
            body: JSON.stringify(bodyRequest)
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/login", requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
    async alterarSenha(novaSenha, token) {
        const requestOptions = {
            headers: vue.$globals.headerPadrao
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + `usuario/alterarSenha?novaSenha=${novaSenha}&token=${token}`, requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
    async listarPerfis() {
        const validado = await this.validarToken();
        if (validado) {
            const requestOptions = {
                method: 'GET',
                headers: vue.$globals.headerPadrao
            };
            const fetchResponse = await fetch(vue.$globals.endpoint + "perfil", requestOptions);
            const data = await fetchResponse.json();

            let response = new Response();
            response.statusCode = fetchResponse.status;
            response.data = data;
            return response;
        }
    }
    async cadastrarNovoUsuarioLogin(usuario) {
        const requestOptions = {
            method: 'POST',
            headers: vue.$globals.headerPadrao,
            body: JSON.stringify(usuario)
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + "usuario/novoUsarioLogin", requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
    async alterarSenhaUsuario(id, senha, retornarUsuarioCompleto) {
        const requestOptions = {
            headers: vue.$globals.headerPadrao
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + `usuario/alterarSenhaUsuario?usuarioId=${id}&senha=${senha}&retornarUsuarioCompleto=${retornarUsuarioCompleto}`, requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
    async consultarUsuarioPorLoginOuEmail(busca) {
        const requestOptions = {
            headers: vue.$globals.headerPadrao
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + `usuario/consultarUsuarioPorLoginOuEmail?busca=${busca}`, requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
    async getById(id) {
        const requestOptions = {
            headers: vue.$globals.headerPadrao
        };
        const fetchResponse = await fetch(vue.$globals.endpoint + `usuario/${id}`, requestOptions);
        const data = await fetchResponse.json();

        let response = new Response();
        response.statusCode = fetchResponse.status;
        response.data = data;
        return response;
    }
}

export default UsuarioService;