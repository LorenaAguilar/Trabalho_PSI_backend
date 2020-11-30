﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSG_API.Business;
using SSG_API.Models;
using System.Threading.Tasks;

namespace SSG_API.Controllers
{
    [ApiController]
    [Route("api/FacebookSignIn")]
    public class FacebookSignInController : ControllerBase
    {
        private FacebookSignInService _facebookSignInService;

        public FacebookSignInController(FacebookSignInService facebookSignInService)
        {
            _facebookSignInService = facebookSignInService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Post([FromBody] FacebookSignInModel accessToken)
        {
            if (accessToken != null && accessToken.AccessToken != null)
            {
                return await _facebookSignInService.LogIn(accessToken.AccessToken);
            }

            return Unauthorized(new { Message = "Token inválido" });
        }
    }
}
