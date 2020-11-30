using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Security;
using System;
using System.Linq;
using SSG_API.Business;

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ContratantesController : ControllerBase
    {
        private readonly ContratanteService _contratanteService;

        public ContratantesController(ContratanteService contratanteService)
        {
            _contratanteService = contratanteService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] string email = "", [FromQuery] string id = "", [FromQuery] string userName = "")
        {
            object result = null;

            if (id != string.Empty)
                result = _contratanteService.GetById(new Guid(id));
            else if (email != string.Empty)
                result = _contratanteService.GetByEmail(email);
            else if (userName != null)
                result = _contratanteService.GetByUserName(userName);

            if (result != null)
                return Ok(result);

            return NotFound();
        }
    }
}
