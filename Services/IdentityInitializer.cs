using Microsoft.AspNetCore.Identity;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Security;
using System;

namespace SSG_API.Services
{
    public class IdentityInitializer
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityInitializer(
            IdentityDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(Roles.Admin).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.Admin)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {Roles.Admin}.");
                    }
                }
                if (!_roleManager.RoleExistsAsync(Roles.Cliente).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.Cliente)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {Roles.Cliente}.");
                    }
                }
                if (!_roleManager.RoleExistsAsync(Roles.Prestador).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.Prestador)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {Roles.Prestador}.");
                    }
                }

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "admin_apiprodutos",
                        Email = "admin-apiprodutos@teste.com.br",
                        EmailConfirmed = true
                    }, "AdminAPIProdutos01!", Roles.Admin);

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "usrinvalido_apiprodutos",
                        Email = "usrinvalido-apiprodutos@teste.com.br",
                        EmailConfirmed = true
                    }, "UsrInvAPIProdutos01!");

                CreateUser(
                    new ApplicationUser()
                    {
                        Id = "04EE39EB-F712-43FB-946D-5DA2318ABDD7",
                        UserName = "PrestadorExemplo",
                        Email = "prestadorexemplo@teste.com.br",
                        EmailConfirmed = true
                    }, "PrestadorExemplo01!", Roles.Prestador);

                //CreateUser(
                //    new ApplicationUser()
                //    {
                //        Id = "872C723E-828C-4873-988E-1E581C4E4CE9",
                //        UserName = "PrestadorExemplo1",
                //        Email = "prestadorexemplo@teste.com.br",
                //        EmailConfirmed = true
                //    }, "PrestadorExemplo02!", Roles.Prestador);

                //CreateUser(
                //    new ApplicationUser()
                //    {
                //        Id = "D541E30B-CBBC-4FD4-9538-9FFD59CF501D",
                //        UserName = "PrestadorExemplo2",
                //        Email = "prestadorexemplo@teste.com.br",
                //        EmailConfirmed = true
                //    }, "PrestadorExemplo03!", Roles.Prestador);

                //CreateUser(
                //    new ApplicationUser()
                //    {
                //        Id = "1D90D272-944A-44F1-AD2B-68ED7AA0C622",
                //        UserName = "PrestadorExemplo3",
                //        Email = "prestadorexemplo@teste.com.br",
                //        EmailConfirmed = true
                //    }, "PrestadorExemplo04!", Roles.Prestador);

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "ClienteExemplo",
                        Email = "clienteexemplo@teste.com.br",
                        EmailConfirmed = true
                    }, "ClienteExemplo01!", Roles.Cliente);
            }
        }
        private void CreateUser(
            ApplicationUser user,
            string password,
            string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (resultado.Succeeded &&
                    !String.IsNullOrWhiteSpace(initialRole))
                {
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
            }
        }
    }
}