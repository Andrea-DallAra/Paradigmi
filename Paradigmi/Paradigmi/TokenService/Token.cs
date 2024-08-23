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
            // Convalida le impostazioni di configurazione, recupera i dati dal file json
            var key = _config["Jwt:Key"];
            var origine = _config["Jwt:Issuer"];
            var timer = _config.GetValue<int>("Jwt:ExpiryMinutes", 120);

            // Genera un'eccezione se la chiave o l'origine mancano
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(origine))
            {
                throw new ArgumentException("Le impostazioni di configurazione JWT sono mancanti o non valide.");
            }

            // Crea una chiave di sicurezza dalla chiave segreta e la utilizza per creare credenziali di firma con l'algoritmo HMAC SHA-256.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Definisce le claims da includere nel token JWT, come l'ID dell'utente, l'email e il nome.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Name, user.nome)
            };

            // Crea un oggetto JwtSecurityToken utilizzando l'origine, le claims e il tempo di scadenza.
            var token = new JwtSecurityToken(
                origine,
                origine,
                claims,
                expires: DateTime.Now.AddMinutes(timer),
                signingCredentials: credentials);

            // Converte l'oggetto JwtSecurityToken in una rappresentazione stringa del token e lo restituisce.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
