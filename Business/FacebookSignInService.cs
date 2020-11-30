using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SSG_API.Models.External;
using Microsoft.AspNetCore.Identity;
using SSG_API.Domain;
using System;
using SSG_API.Models;
using System.Security.Claims;
using SSG_API.Security;
using SSG_API.Data;

namespace SSG_API.Business
{
    public class FacebookSignInService
    {
        private string _accessToken;
        private string _appId;
        private UserManager<ApplicationUser> _userManager;
        private AccessManager _accessManager;
        private IdentityDbContext _identityDbContext;
        private ApplicationDbContext _applicationDbContext;

        public FacebookSignInService(
            UserManager<ApplicationUser> userManager,
            AccessManager accessManager,
            FacebookConfigurations facebookConfigurations,
            IdentityDbContext identityDbContext,
            ApplicationDbContext applicationDbContext)
        {
            _accessToken = facebookConfigurations.AcessToken;
            _appId = facebookConfigurations.AppId;
            _userManager = userManager;
            _accessManager = accessManager;
            _identityDbContext = identityDbContext;
            _applicationDbContext = applicationDbContext;
        }

        private async Task<bool> IsValidAccessToken(string inputToken)
        {
            using (var client = new HttpClient())
            {
                string url = $@"https://graph.facebook.com/debug_token?input_token={inputToken}&access_token={_accessToken}";

                var resultContent = await (await client.GetAsync(url)).Content.ReadAsStringAsync();

                var resultContentObject = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(resultContent);


                bool isValid = resultContentObject?.Data?.IsValid == null ? false : resultContentObject.Data.IsValid;
                bool isSsgGenerated = resultContentObject?.Data?.AppId == null ? false : resultContentObject.Data.AppId == _appId;

                return isValid && isSsgGenerated;
            }
        }

        private async Task<FacebookUserInfoResult> GetUserData(string inputToken)
        {
            using (var client = new HttpClient())
            {
                string url = $@"https://graph.facebook.com/v9.0/me?fields=name,email,picture&access_token={inputToken}";

                var resultContent = await (await client.GetAsync(url)).Content.ReadAsStringAsync();

                var resultContentObject = JsonConvert.DeserializeObject<FacebookUserInfoResult>(resultContent);

                return resultContentObject;
            }
        }

        public async Task<object> LogIn(FacebookSignInModel signInModel)
        {
            var isValidToken = await IsValidAccessToken(signInModel.AccessToken);

            if (!isValidToken)
            {
                return new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar"
                };
            }

            var userData = await GetUserData(signInModel.AccessToken);

            var user = await _userManager.FindByEmailAsync(userData.Email);

            if (user == null)
            {
                var appUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = userData.Email,
                    UserName = userData.Email,
                    NomeCompleto = userData.Name,
                    LinkFoto = userData.FacebookPicture.Data.Url.AbsoluteUri,
                    Cpf = signInModel.Cpf,
                    Endereco = signInModel.Endereco,
                    Telefone = signInModel.Telefone,
                };

                var creationResult = await _userManager.CreateAsync(appUser);

                if (!creationResult.Succeeded)
                {
                    return new
                    {
                        Authenticated = false,
                        Message = "Falha ao criar o usuário"
                    };
                }



                var createdUser = await _userManager.FindByEmailAsync(appUser.Email);

                if (signInModel.Tipo == "PRESTADOR")
                {
                    Prestador prestador = new Prestador
                    {
                        Biografia = signInModel.Biografia,
                        User = createdUser,
                        Id = Guid.NewGuid()
                    };

                    await _userManager.AddToRoleAsync(createdUser, Roles.Prestador);

                    _identityDbContext.SaveChanges();
                    _applicationDbContext.Add<Prestador>(prestador);

                    _applicationDbContext.Add<ServicoPrestado>(
                        new ServicoPrestado()
                        {
                            Servico = _applicationDbContext.Find<Servico>(signInModel.Servico),
                            Prestador = _applicationDbContext.Find<Prestador>(createdUser),
                            Unidade = _applicationDbContext.Find<UnidadeDeCobranca>(signInModel.UnidadeDeCobranca),
                            Preco = signInModel.Preco
                        }
                    );

                    _applicationDbContext.SaveChanges();

                    return "Succeeded";
                }
                else if (signInModel.Tipo == "CLIENTE")
                {
                    Contratante contratante = new Contratante
                    {
                        Id = Guid.NewGuid(),
                        User = createdUser
                    };

                    await _userManager.AddToRoleAsync(createdUser, Roles.Cliente);

                    _identityDbContext.SaveChanges();
                    _applicationDbContext.Add<Contratante>(contratante);
                    _applicationDbContext.SaveChanges();

                    return "Succeeded";
                }

                return new
                {
                    Authenticated = true,
                    Message = "Usuário criado com sucesso",
                    AccessToken = _accessManager.GenerateToken(user).AccessToken
                };
            }

            return _accessManager.GenerateToken(user);
        }
    }
}
