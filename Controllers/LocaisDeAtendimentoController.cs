using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class LocaisDeAtendimentoController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LocaisDeAtendimentoController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = _applicationDbContext.LocaisDeAtendimento.ToList<object>();

            if (result.Any())
                return Ok(result);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            var found = _applicationDbContext.LocaisDeAtendimento.Find(id);

            if (found != null)
            {
                var result = new LocaisDeAtendimentoModel()
                {
                    Prestador = found.Prestador.Id,
                    Cidade = found.Cidade,
                    Estado = found.Estado
                };
                
                return Ok(result);
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult<object> Post([FromBody] LocaisDeAtendimentoModel value)
        {
            var result = _applicationDbContext.Add<LocaisDeAtendimento>(
                new LocaisDeAtendimento()
                {
                    Cidade = value.Cidade,
                    Estado = value.Estado,
                    Prestador = _applicationDbContext.Find<Prestador>(value.Prestador)
                }).Entity;
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
