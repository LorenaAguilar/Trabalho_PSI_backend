using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Business;

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListarServicosController : ControllerBase
    {
        private readonly ListarServicosService _listarServicosService;

        public ListarServicosController(ListarServicosService listarServicosService)
        {
            _listarServicosService = listarServicosService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<object> Get()
        {
            var results = _listarServicosService.List();

            if (results != null && results.Any())
                return Ok(results);
            else
                return NotFound(new { Message = "Nenhum serviço prestado cadastrado" });
        }
    }
}
