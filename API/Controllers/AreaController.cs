using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Business.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : BaseController
    {
        public AreaController(ILogger<AreaController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IAreaService _areaService)
        {
            try
            {
                return Ok(await _areaService.GetAllAsync());
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar area.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }
    }
}
