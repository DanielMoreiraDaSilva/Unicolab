using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using Data.Models.Filtros;
using Business.TransferObjects;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkImportanteController : BaseController
    {
        public LinkImportanteController(ILogger<LinkImportanteController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] ILinkImportanteService _linkImportanteService)
        {
            try
            {
                return Ok(await _linkImportanteService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar link importante.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("listarLinks")]
        public async Task<IActionResult> ListarUsuarios(
            [FromBody] FiltroLinkImportante filtro,
            [FromServices] ILinkImportanteService _linkImportanteService)
        {
            try
            {
                return Ok(await _linkImportanteService.ListarLinksAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao listar links importantes.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LinkImportanteDto linkImportante,
            [FromServices] ILinkImportanteService _linkImportanteService)
        {
            try
            {
                var validacaoLink = await _linkImportanteService.LinkExistenteAsync(linkImportante.Id, linkImportante.Url);

                var validacaoTitulo = await _linkImportanteService.TituloExistenteAsync(linkImportante.Id, linkImportante.Titulo);

                if(validacaoLink && validacaoTitulo)
                    return Conflict(new MensagemErroDto(" j� cadastrado.", new { campoErrado1 = "Link", CampoErrado2 = "T�tulo" }));

                if (validacaoLink)
                    return Conflict(new MensagemErroDto("Link j� cadastrado.", new { campoErrado = "url" }));

                if (validacaoTitulo)
                    return Conflict(new MensagemErroDto("T�tulo j� cadastrado.", new { campoErrado = "titulo" }));

                await _linkImportanteService.AddAsync(linkImportante);
                LogExecucao($"LinkImportante.Add : T�tulo - {linkImportante.Titulo}");
                return Ok(new MensagemSucessoDto("link importante inserido com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar link importante.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] LinkImportanteDto linkImportante,
            [FromServices] ILinkImportanteService _linkImportanteService)
        {
            try
            {
                var validacaoLink = await _linkImportanteService.LinkExistenteAsync(linkImportante.Id, linkImportante.Url);

                var validacaoTitulo = await _linkImportanteService.TituloExistenteAsync(linkImportante.Id, linkImportante.Titulo);

                if (validacaoLink)
                    return Conflict(new MensagemErroDto("N�o � poss�vel atualizar para um link j� existente."));

                if (validacaoTitulo)
                    return Conflict(new MensagemErroDto("N�o � poss�vel atualizar para um t�tulo j� existente."));

                await _linkImportanteService.UpdateAsync(linkImportante);
                LogExecucao($"LinkImportante.Update : ID - {linkImportante.Id} / T�tulo - {linkImportante.Titulo}");
                return Ok(new MensagemSucessoDto("link importante atualizado com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar link importante.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
