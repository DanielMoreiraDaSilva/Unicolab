using Business.Interfaces;
using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Business.TransferObjects;
using Data.Models.Filtros;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OportunidadeController : BaseController
    {
        public OportunidadeController(ILogger<OportunidadeController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IOportunidadeService _oportunidadeService)
        {
            try
            {
                return Ok(await _oportunidadeService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar oportunidade.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
        
        [HttpGet("curso/{cursoId}")]
        public async Task<IActionResult> Get([FromServices] IOportunidadeService _oportunidadeService,
            string cursoId)
        {
            try
            {
                return Ok(await _oportunidadeService.GetAllByCurso(cursoId));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar oportunidades do curso");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("listarOportunidades")]
        public async Task<IActionResult> ListarOportunidadesAsync(
           [FromBody] FiltroOportunidade filtro,
           [FromServices] IOportunidadeService _oportunidadeService)
        {
            try
            {
                return Ok(await _oportunidadeService.ListarOportunidadesAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao listar oportunidades.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] OportunidadeDto oportunidade,
        [FromServices] IOportunidadeService _oportunidadeService)
        {
            try
            {
                await _oportunidadeService.UpdateAsync(oportunidade);
                LogExecucao($"oportunidade.Update : ID - {oportunidade.Id} / Título - {oportunidade.Titulo}");
                return Ok(new MensagemSucessoDto("Oportunidade atualizada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar oportunidade.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OportunidadeDto oportunidade,
            [FromServices] IOportunidadeService _oportunidadeService)
        {
            try
            {
                await _oportunidadeService.AddAsync(oportunidade);
                LogExecucao($"oportunidade.Add : Título - {oportunidade.Titulo}");
                return Ok(new MensagemSucessoDto("Oportunidade inserida com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar oportunidade.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id,
            [FromServices] IOportunidadeService _oportunidadeService)
        {
            try
            {
                await _oportunidadeService.DeleteAsync(id);
                LogExecucao($"Oportunidade.Delete : ID - {id}");
                return Ok(new MensagemSucessoDto("oportunidade deletada com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao deletar oportunidade.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
