using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Models.Filtros;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DuvidaController : BaseController
    {
        public DuvidaController(ILogger<DuvidaController> logger) : base(logger) { }

        [HttpPost("listarDuvidas")]
        public async Task<IActionResult> Get([FromServices] IDuvidaService _duvidaService,
            [FromBody] FiltroDuvida filtro)
        {
            try
            {
                return Ok(await _duvidaService.ListarDuvidasAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar link importante.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DuvidaDto duvida,
            [FromServices] IDuvidaService _duvidaService)
        {
            try
            {
                await _duvidaService.AddAsync(duvida);
                LogExecucao($"LinkImportante.Add : {duvida.Pergunta}");
                return Ok(new MensagemSucessoDto("dúvida inserida com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar dúvida.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] DuvidaDto duvida,
            [FromServices] IDuvidaService _duvidaService)
        {
            try
            {
                await _duvidaService.UpdateAsync(duvida);
                LogExecucao($"LinkImportante.Update : ID - {duvida.Id} / {duvida.Pergunta}");
                return Ok(new MensagemSucessoDto("dúvida atualizada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar dúvida.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id,
            [FromServices] IDuvidaService _duvidaService)
        {
            try
            {
                await _duvidaService.DeleteAsync(id);
                LogExecucao($"Duvida.Delete : ID - {id}");
                return Ok(new MensagemSucessoDto("duvida deletada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao deletar duvida.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
