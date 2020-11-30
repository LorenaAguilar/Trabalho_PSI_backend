using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using SSG_API.Data;
using SSG_API.Domain;

namespace SSG_API.Business
{
    public class ListarServicosService
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly ApplicationDbContext _applicationDbContext;

        public ListarServicosService(IdentityDbContext identityDbContext, ApplicationDbContext applicationDbContext)
        {
            _identityDbContext = identityDbContext;
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<object> List()
        {
            var results = (
            from p in _applicationDbContext.Prestadores
            join sp in _applicationDbContext.ServicosPrestados on p.Id equals sp.Prestador.Id
            join s in _applicationDbContext.Servicos on sp.Servico.Id equals s.Id
            join l in _applicationDbContext.LocaisDeAtendimento on p.Id equals l.Prestador.Id

            select new 
            {
                nomeCompleto = p.User.NomeCompleto,
                foto = p.User.LinkFoto,
                categoriaServico = s.Nome,
                cidade = l.Cidade,
                preco = sp.Preco,
                biografia = p.Biografia,
                prestador = p.Id,
                servico = s.Id,
                servicoprestado = sp.Id
            }
            ).ToList<object>();

            return results;
        }
    }
}