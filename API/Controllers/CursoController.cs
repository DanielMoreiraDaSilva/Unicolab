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
    public class CursoController : BaseController
    {
        public CursoController(ILogger<CursoController> logger) : base(logger) { }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> Get([FromServices] ICursoService _cursoService, string usuarioId)
        {
            try
            {
                return Ok(await _cursoService.GetAllAsync(usuarioId));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar curso.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
