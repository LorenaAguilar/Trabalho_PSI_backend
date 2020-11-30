using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SSG_API.Models;
using SSG_API.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using SSG_API.Data;
using Microsoft.AspNetCore.Server.HttpSys;

namespace SSG_API.Security
{
    public class AccessManager
    {
        private UserManager<ApplicationUser> _userManager;
        private IdentityDbContext _identityDbContext;
        private ApplicationDbContext _applicationDbContext;
        private SignInManager<ApplicationUser> _signInManager;
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;

        public AccessManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations,
            IdentityDbContext identityDbContext,
            ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
            _identityDbContext = identityDbContext;
            _applicationDbContext = applicationDbContext;
        }

        public bool ValidateLoginCredentials(SignInUserModel user)
        {
            bool credenciaisValidas = false;
            if (user != null && !String.IsNullOrWhiteSpace(user.Email))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = _userManager
                    .FindByNameAsync(user.Email).Result;
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = _signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if (resultadoLogin.Succeeded)
                    {
                        // Verifica se o usuário em questão possui
                        // a role Acesso-APIProdutos
                        //credenciaisValidas = _userManager.IsInRoleAsync(
                        //    userIdentity, Roles.Admin).Result
                        //    || _userManager.IsInRoleAsync(userIdentity, Roles.Cliente).Result
                        //    || _userManager.IsInRoleAsync(userIdentity, Roles.Prestador).Result;

                        credenciaisValidas = true;
                    }
                }
            }

            return credenciaisValidas;
        }

        public string ValidateSignUp(SignUpUserModel user)
        {
            if
            (
                user != null
                && !String.IsNullOrWhiteSpace(user.Email)
                && !String.IsNullOrWhiteSpace(user.Biografia)
                && !String.IsNullOrWhiteSpace(user.Endereco)
                && !String.IsNullOrWhiteSpace(user.LinkFoto)
                && !String.IsNullOrWhiteSpace(user.NomeCompleto)
                && !String.IsNullOrWhiteSpace(user.Password)
                && !String.IsNullOrWhiteSpace(user.Telefone)
                && !String.IsNullOrWhiteSpace(user.Tipo)
            )
            {
                var applicationUser =
                    new ApplicationUser
                    {
                        Email = user.Email,
                        EmailConfirmed = true,
                        UserName = user.Email,
                        //Biografia = user.Biografia,
                        Endereco = user.Endereco,
                        LinkFoto = user.LinkFoto,
                        NomeCompleto = user.NomeCompleto,
                        Telefone = user.Telefone,
                        Cpf = user.Cpf
                    };
                if (_userManager.FindByEmailAsync(user.Email.Trim().ToUpper()).Result == null)
                {
                    var creationResult = _userManager.CreateAsync(applicationUser, user.Password).Result;
                    if (creationResult.Succeeded)
                    {
                        var createdUser = _userManager.FindByEmailAsync(user.Email).Result;

                        if (user.Tipo == "PRESTADOR")
                        {
                            Prestador prestador = new Prestador
                            {
                                Biografia = user.Biografia,
                                User = createdUser,
                                Id = Guid.NewGuid()
                            };

                            var result = _userManager.AddToRoleAsync(createdUser, Roles.Prestador).Result;

                            _identityDbContext.SaveChanges();
                            _applicationDbContext.Add<Prestador>(prestador);
                            _applicationDbContext.SaveChanges();

                            return "Succeeded";
                        }
                        else if (user.Tipo == "CLIENTE")
                        {
                            Contratante contratante = new Contratante
                            {
                                Id = Guid.NewGuid(),
                                User = applicationUser
                            };

                            var result = _userManager.AddToRoleAsync(createdUser, Roles.Cliente).Result;

                            _identityDbContext.SaveChanges();
                            _applicationDbContext.Add<Contratante>(contratante);
                            _applicationDbContext.SaveChanges();

                            return "Succeeded";
                        }
                    }
                }
                else
                {
                    return "User already exists";
                }
            }

            return "Failed";
        }

        public Token GenerateToken(SignInUserModel user, ClaimsPrincipal claims)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Email, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);


            return new Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK",
                Roles = _userManager.GetRolesAsync(_userManager.FindByNameAsync(user.Email).Result).Result.ToArray<String>()
            };
        }

        public Token GenerateToken(ApplicationUser user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Email, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);


            return new Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK",
                Roles = _userManager.GetRolesAsync(_userManager.FindByNameAsync(user.Email).Result).Result.ToArray<String>()
            };
        }
    }
}