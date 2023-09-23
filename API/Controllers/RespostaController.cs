using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Business.TransferObjects;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespostaController : BaseController
    {
        public RespostaController(ILogger<RespostaController> logger) : base(logger) { }

        [HttpGet("duvida/{duvidaId}")]
        public async Task<IActionResult> GetByUsuarioId([FromServices] IRespostaService _respostaService,
            string duvidaId)
        {
            try
            {
                return Ok(await _respostaService.GetAllByDuvidaId(duvidaId));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar respostas por id da dúvida.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RespostaDto resposta,
            [FromServices] IRespostaService _respostaService)
        {
            try
            {
                await _respostaService.AddAsync(resposta);
                LogExecucao($"Resposta.Add : {resposta.Descricao}");
                return Ok(new MensagemSucessoDto("resposta inserida com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar resposta.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] RespostaDto resposta,
            [FromServices] IRespostaService _respostaService)
        {
            try
            {
                await _respostaService.UpdateAsync(resposta);
                LogExecucao($"Resposta.Update : ID - {resposta.Id} / {resposta.Descricao}");
                return Ok(new MensagemSucessoDto("resposta atualizada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar resposta.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id,
            [FromServices] IRespostaService _respostaService)
        {
            try
            {
                await _respostaService.DeleteAsync(id);
                LogExecucao($"Resposta.Delete : ID - {id}");
                return Ok(new MensagemSucessoDto("resposta deletada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao deletar resposta.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
