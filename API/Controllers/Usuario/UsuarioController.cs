using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.TransferObjects;
using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Data.Models.Filtros;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        public UsuarioController(ILogger<UsuarioController> logger) : base(logger) { }

        [HttpGet("listarUsuariosAtivos")]
        public async Task<IActionResult> ListarUsuariosAtivos(
            [FromServices] IUsuarioService _usuarioService,
            [FromQuery] bool listaCompleta)
        {
            try
            {
                return Ok(await _usuarioService.ListarUsuariosAtivosAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar usuários.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                return Ok(await _usuarioService.GetAsync(id));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar usuário.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto login,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                var usuario = await _usuarioService.LoginAsync(login.Login, login.Senha);
                if (usuario != null)
                    return Ok(usuario);
                else
                    return Unauthorized(new MensagemErroDto(Resources.USUARIO_NAO_AUTORIZADO));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao efetuar o login.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("alterarSenha")]
        public async Task<IActionResult> AlterarSenha(string novaSenha,
            string token,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                var usuarioEncontrado = await _usuarioService.AlterarSenhaAsync(novaSenha, token);
                if (usuarioEncontrado)
                    return Ok(usuarioEncontrado);
                else
                    return BadRequest(new MensagemErroDto(Resources.USUARIO_NAO_ENCONTRADO));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao alterar a senha.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("alterarSenhaUsuario")]
        public async Task<IActionResult> AlterarSenhaUsuarioWeb(
            [FromServices] IUsuarioService _usuarioService,
            string usuarioId,
            string senha)
        {
            try
            {
                return Ok(await _usuarioService.AlterarSenhaUsuarioAsync(usuarioId, senha));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao alterar a senha.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDto usuario,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                var validacao = await _usuarioService.UsuarioExistenteAsync(usuario.Id, usuario.Nome);

                var validacaoLogin = await _usuarioService.LoginExistenteAsync(usuario.Login);

                if (validacao)
                    return Conflict(new MensagemErroDto("Usuário já cadastrado.", new { campoErro = "nome" }));

                if (validacaoLogin)
                    return Conflict(new MensagemErroDto("Login já cadastrado.", new { campoErro = "login" }));

                await _usuarioService.AddAsync(usuario);
                LogExecucao($"Usuario.Add : Nome - {usuario.Nome}");
                return Ok(new MensagemSucessoDto("usuário inserido com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar usuário.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UsuarioDto usuario,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                var validacao = await _usuarioService.UsuarioExistenteAsync(usuario.Id, usuario.Nome);

                if (validacao)
                    return Conflict(new MensagemErroDto("Não é possível atualizar para um nome de usuário já existente."));

                await _usuarioService.UpdateAsync(usuario);
                LogExecucao($"Usuario.Update : ID - {usuario.Id} / Nome - {usuario.Nome}");
                return Ok(new MensagemSucessoDto("usuário atualizado com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar usuário.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("listarUsuarios")]
        public async Task<IActionResult> ListarUsuarios(
            [FromBody] FiltroUsuario filtro,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                return Ok(await _usuarioService.ListarUsuariosAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao listar usuários.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("listarUsuariosContatos")]
        public async Task<IActionResult> ListarUsuariosContatos(
           [FromBody] FiltroUsuarioContato filtro,
           [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                return Ok(await _usuarioService.ListarUsuariosContatosAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao listar usuários.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        //[HttpPost("usuariosExcel")]
        //public async Task<IActionResult> UsuariosExcel(
        //    [FromBody] FiltroUsuario filtro,
        //    [FromServices] IUsuarioService _usuarioService,
        //    [FromServices] IWebHostEnvironment _hostingEnvironment)
        //{
        //    string caminho = _hostingEnvironment.ContentRootPath;
        //    var stream = await _usuarioService.UsuariosExcelAsync(filtro, caminho);
        //    return this.File(
        //        fileContents: stream.ToArray(),
        //        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        fileDownloadName: "Usuarios.xlsx"
        //    );
        //}

        [HttpPost("novoUsarioLogin")]
        public async Task<IActionResult> NovoUsuarioLogin([FromBody] UsuarioDto usuario,
            [FromServices] IUsuarioService _usuarioService)
        {
            try
            {
                var loginExistente = await _usuarioService.LoginExistenteAsync(usuario.Login);
                var emailExistente = await _usuarioService.EmailExistenteAsync(usuario.Email);

                if (loginExistente)
                    return Conflict(new MensagemErroDto("Login já cadastrado", new { campoErro = "login" }));

                if (emailExistente)
                    return Conflict(new MensagemErroDto("E-mail já cadastrado", new { campoErro = "email" }));

                var retorno = await _usuarioService.CadastrarUsuarioLoginAsync(usuario);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar usuário.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("consultarUsuarioPorLoginOuEmail")]
        public async Task<IActionResult> ConsultarUsuarioPorLoginOuEmail(string busca,
            [FromServices] IUsuarioService _usuarioService,
            [FromServices] IWebHostEnvironment _hostingEnvironment)
        {
            try
            {
                string caminho = _hostingEnvironment.ContentRootPath;
                var resultado = await _usuarioService.ConsultarUsuarioPorLoginOuEmailAsync(busca, caminho);
                if (resultado != null)
                    return Ok(resultado);
                else
                    return NotFound(new MensagemErroDto("Nenhum usuário encontrado"));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao consultar o usuário por login ou e-mail.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
