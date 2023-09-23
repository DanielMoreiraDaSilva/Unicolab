using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : BaseController
    {
        public PerfilController(ILogger<PerfilController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IPerfilService _perfilService)
        {
            try
            {
                return Ok(await _perfilService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar perfil.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
