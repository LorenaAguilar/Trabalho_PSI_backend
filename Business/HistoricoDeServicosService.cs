using Microsoft.AspNetCore.Identity;
using SSG_API.Data;
using SSG_API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SSG_API.Business
{
    public class HistoricoDeServicosService
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly ApplicationDbContext _applicationDbContext;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public HistoricoDeServicosService(IdentityDbContext identityDbContext, ApplicationDbContext applicationDbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _identityDbContext = identityDbContext;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IEnumerable<object> List(string userName)
        {
            //var user = _identityDbContext.Find<ApplicationUser>(userId);
            var user2 = _userManager.FindByNameAsync(userName).Result;

            if (user2 != null)
            {
                bool isPrestador = (
                    from p in _applicationDbContext.Prestadores
                    where p.User.Id == user2.Id
                    select p.Id
                ).Any();

                if (isPrestador)
                {
                    return (
                        from p in _applicationDbContext.Prestadores
                        join l in _applicationDbContext.LocaisDeAtendimento on p.Id equals l.Prestador.Id
                        join sp in _applicationDbContext.ServicosPrestados on p.Id equals sp.Prestador.Id
                        join s in _applicationDbContext.Servicos on sp.Servico.Id equals s.Id
                        join os in _applicationDbContext.OrdensDeServico on p.Id equals os.Prestador.Id
                        join c in _applicationDbContext.Contratantes on os.Contratante.Id equals c.Id
                        where p.User.Id == user2.Id
                        select new
                        {
                            OrdemDeServico = os.Id,
                            PrestadorNome = p.User.NomeCompleto,
                            PrestadorFoto = p.User.LinkFoto,
                            ContratanteNome = c.User.NomeCompleto,
                            ContratanteFoto = c.User.LinkFoto,
                            Servico = s.Nome,
                            Regiao = l.Cidade,
                            Preco = sp.Preco,
                            Biografia = p.Biografia,
                            Data = os.DataPrestacao,
                            Situacao = os.Status
                        }
                    );
                }
                else
                {
                    return (
                    from c in _applicationDbContext.Contratantes
                    join os in _applicationDbContext.OrdensDeServico on c.Id equals os.Contratante.Id
                    join p in _applicationDbContext.Prestadores on os.Prestador.Id equals p.Id
                    join l in _applicationDbContext.LocaisDeAtendimento on p.Id equals l.Prestador.Id
                    join sp in _applicationDbContext.ServicosPrestados on p.Id equals sp.Prestador.Id
                    join s in _applicationDbContext.Servicos on sp.Servico.Id equals s.Id
                    where c.User.Id == user2.Id
                    select new
                    {
                        OrdemDeServico = os.Id,
                        PrestadorNome = p.User.NomeCompleto,
                        PrestadorFoto = p.User.LinkFoto,
                        ContratanteNome = c.User.NomeCompleto,
                        ContratanteFoto = c.User.LinkFoto,
                        Servico = s.Nome,
                        Regiao = l.Cidade,
                        Preco = sp.Preco,
                        Biografia = p.Biografia,
                        Data = os.DataPrestacao,
                        Situacao = os.Status
                    }
                );
                }
            }
            return null;
        }
    }
}
