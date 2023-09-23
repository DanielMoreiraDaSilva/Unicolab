using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Business.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : BaseController
    {
        public MateriaController(ILogger<MateriaController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IMateriaService _materiaService)
        {
            try
            {
                return Ok(await _materiaService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar a materia.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId([FromServices] IMateriaService _materiaService,
            string usuarioId)
        {
            try
            {
                return Ok(await _materiaService.GetAllByUsuarioAsync(usuarioId));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar a materias pos usuário.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
