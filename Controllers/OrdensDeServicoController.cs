using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSG_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class OrdensDeServicoController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrdensDeServicoController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = _applicationDbContext.OrdensDeServico.ToList<OrdemDeServico>();
            var results = new List<object>();
            foreach (var item in result)
            {
                results.Add(new
                {
                    Id = item.Id,
                    Contratante = item.Contratante?.Id,
                    Prestador = item.Prestador?.Id,
                    ServicoPrestado = item.ServicoPrestado?.Id,
                    FormaPagamento = item.FormaPagamento,
                    DataPrestacao = item.DataPrestacao,
                    Endereco = item.Endereco,
                    Preco = item.Preco,
                    Resumo = item.Resumo,
                    Status = item.Status
                });
            }

            if (result.Any())
                return Ok(result);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<object> Get(Guid id)
        {
            var result = _applicationDbContext.Servicos.Find(id);

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<object> Post([FromBody] OrdemDeServicoModel value)
        {
            var result = _applicationDbContext.Add<OrdemDeServico>(
                new OrdemDeServico()
                {
                    Prestador = _applicationDbContext.Prestadores.Find(value.Prestador),
                    Contratante = _applicationDbContext.Contratantes.Find(value.Contratante),
                    ServicoPrestado = _applicationDbContext.ServicosPrestados.Find(value.ServicoPrestado),
                    DataPrestacao = (DateTime)value.Data,
                    Preco = (double)value.Preco,
                    Endereco = value.Endereco,
                    Resumo = value.Resumo,
                    Status = (int)value.Status,
                    FormaPagamento = (int)value.FormaPagamento
                }
            ).Entity;

            _applicationDbContext.SaveChanges();

            return Ok(result);
        }

        [HttpPut]
        public object Put([FromBody] OrdemDeServicoModel value)
        {
            var actual = _applicationDbContext.OrdensDeServico.Find(value?.Id);
            if (actual == null)
            {
                return NotFound();
            }
            else
            {
                actual.Id = (Guid)value.Id;
                actual.Contratante = actual.Contratante;
                actual.Prestador = actual.Prestador;
                actual.ServicoPrestado = actual.ServicoPrestado;
                actual.FormaPagamento = value.FormaPagamento != null ? (int)value.FormaPagamento : actual.FormaPagamento;
                actual.DataPrestacao = value.Data != null ? (DateTime)value.Data : actual.DataPrestacao;
                actual.Endereco = value.Endereco != null ? value.Endereco : actual.Endereco;
                actual.Preco = value.Preco != null ? (double)value.Preco : actual.Preco;
                actual.Resumo = value.Resumo != null ? value.Resumo : actual.Resumo;
                actual.Status = value.Status != null ? (int)value.Status : actual.Status;


                //var result = 
                _applicationDbContext.OrdensDeServico.Update(actual);
                //var response = new {
                //    Id = result.Id,
                //    Contratante = result.Contratante.Id,
                //    Prestador = result.Prestador.Id,
                //    ServicoPrestado =result.ServicoPrestado.Id,
                //    FormaPagamento = result.FormaPagamento,
                //    DataPrestacao = result.DataPrestacao,
                //    Endereco = result.Endereco,
                //    Preco = result.Preco,
                //    Resumo = result.Resumo,
                //    Status = result.Status
                //    };
                _applicationDbContext.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
