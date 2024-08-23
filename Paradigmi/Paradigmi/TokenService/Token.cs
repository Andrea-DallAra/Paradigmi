using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Paradigmi.Classi;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Paradigmi.TokenService
{
    public class Token
    {
        private readonly IConfiguration _config;

        public Token(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Utente user)
        {
           
            var key = _config["Jwt:Key"];
            var origine = _config["Jwt:Issuer"];
            var timer = _config.GetValue<int>("Jwt:ExpiryMinutes", 120);
         
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(origine))
            {
                throw new ArgumentException("Le impostazioni di configurazione JWT sono mancanti o non valide.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Name, user.nome)
            };

            var token = new JwtSecurityToken(
                origine,
                origine,
                claims,
                expires: DateTime.Now.AddMinutes(timer),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
