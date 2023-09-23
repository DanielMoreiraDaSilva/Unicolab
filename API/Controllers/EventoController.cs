using Business.Interfaces;
using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Data.Interfaces;
using Business.TransferObjects;
using Data.Models.Filtros;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : BaseController
    {
        public EventoController(ILogger<EventoController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IEventoService _eventoService)
        {
            try
            {
                return Ok(await _eventoService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar Evento.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventoDto evento,
            [FromServices] IEventoService _eventoService)
        {
            try
            {
                await _eventoService.AddAsync(evento);
                LogExecucao($"evento.Add : Título - {evento.Titulo}");
                return Ok(new MensagemSucessoDto("Evento inserido com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar Evento.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] EventoDto evento,
        [FromServices] IEventoService _eventoService)
        {
            try
            {
                await _eventoService.UpdateAsync(evento);
                LogExecucao($"LinkImportante.Update : ID - {evento.Id} / Título - {evento.Titulo}");
                return Ok(new MensagemSucessoDto("Evento atualizado com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar o Evento.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
        [HttpPost("listarEventos")]
        public async Task<IActionResult> ListarEventosAsync(
           [FromBody] FiltroEvento filtro,
           [FromServices] IEventoService _eventoService)
        {
            try
            {
                return Ok(await _eventoService.ListarEventosAsync(filtro));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao listar eventos.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id,
            [FromServices] IEventoService _eventoService)
        {
            try
            {
                await _eventoService.DeleteAsync(id);
                LogExecucao($"Evento.Delete : ID - {id}");
                return Ok(new MensagemSucessoDto("evento deletado com sucesso.", null));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao deletar evento.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}