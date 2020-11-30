using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Business;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ServicosPrestadosController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ServicosPrestadosController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ServicoPrestado>> Get()
        {
            var servicosPrestados = from sp in _applicationDbContext.ServicosPrestados
                                    select new {
                                        Servico = sp.Servico.Id,
                                        Prestador = sp.Prestador.Id,
                                        Unidade = sp.Unidade.Id,
                                        Preco = sp.Preco
                                        };
            
            List<object> results = new List<object>();
            
            foreach (var item in servicosPrestados)
            {
                results.Add(
                    new ServicoPrestadoModel
                    {
                        Servico = item.Servico,
                        Prestador = item.Prestador,
                        Unidade = item.Unidade,
                        Preco = item.Preco
                    }    
                );
            }

            if (servicosPrestados.Any())
                return Ok(results);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ServicoPrestado> Get(Guid id)
        {
            var found = _applicationDbContext.ServicosPrestados.Find(id);

            if (found != null)
            {
                var result = new ServicoPrestadoModel()
                {
                    Servico = found.Servico.Id,
                    Prestador = found.Prestador.Id,
                    Preco = found.Preco,
                    Unidade = found.Unidade.Id
                };

                return Ok(result);
            }



            return NotFound();
        }

        [HttpPost]
        public ActionResult<ServicoPrestado> Post([FromBody] ServicoPrestadoModel servico)
        {
            var result = _applicationDbContext.Add<ServicoPrestado>(
                new ServicoPrestado()
                {
                    Servico = _applicationDbContext.Find<Servico>(servico.Servico),
                    Prestador = _applicationDbContext.Find<Prestador>(servico.Prestador),
                    Unidade = _applicationDbContext.Find<UnidadeDeCobranca>(servico.Unidade),
                    Preco = servico.Preco
                }).Entity;
            _applicationDbContext.SaveChanges();

            return Ok(result);
        }

        [HttpPut]
        public ActionResult<Resultado> Put([FromBody] ServicoPrestado servico)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public ActionResult<Resultado> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
