using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Data;
using SSG_API.Domain;

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class UnidadesDeCobrancaController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnidadesDeCobrancaController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = _applicationDbContext.UnidadesDeCobranca.ToList<object>();

            if (result.Any())
                return Ok(result);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<string> Get(Guid id)
        {
            var result = _applicationDbContext.UnidadesDeCobranca.Find(id);

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<object> Post([FromBody] string unidade)
        {
            var result = _applicationDbContext.Add<UnidadeDeCobranca>(new UnidadeDeCobranca() { Unidade = unidade }).Entity;
            _applicationDbContext.SaveChanges();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
