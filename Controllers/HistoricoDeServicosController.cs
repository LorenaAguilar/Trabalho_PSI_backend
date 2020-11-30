using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Business;
using System;
using System.Linq;

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class HistoricoDeServicosController : ControllerBase
    {
        private readonly HistoricoDeServicosService _historicoDeServicosService;

        public HistoricoDeServicosController(HistoricoDeServicosService historicoDeServicosService)
        {
            _historicoDeServicosService = historicoDeServicosService;
        }

        [HttpGet]
        public ActionResult<object> Get()
        {
            var results = _historicoDeServicosService.List(HttpContext.User.Identity.Name);

            if (results != null && results.Any())
                return Ok(results);
            else
                return NotFound(new { Message = "Nenhum serviço prestado cadastrado" });
        }
    }
}
