using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autenticacao.Models;
using Autenticacao.ProviderJWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autenticacao.Controllers
{
    public class TokenController : Controller
    {
        [Route("/api/login")]
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public IActionResult CreateToken([FromBody] User user)
        {

            if (user.UserName != "marcus" || user.Password != "1234")
                return Unauthorized();

            var token = new TokenJwtBuilder()
                .AddSecurityKey(ProviderJWT.JwtSecurityKey.Create("Secret_Key_Iterup"))
                .AddSubject("Marcus Vinicius Pereira")
                .AddIssuer("Iterup.Security.Bearer")
                .AddAudience("Iterup.Security.Bearer")
                .Addclaim("UsuarioApiNumero", "1")
                .AddExpiryInMinutes(5)
                .Builder();

            return Ok(token.value);
        }
    }
}