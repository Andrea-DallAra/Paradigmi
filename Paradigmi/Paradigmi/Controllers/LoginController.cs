﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Paradigmi.Classi;
using Paradigmi.TokenService;
using Paradigmi.Dati;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Paradigmi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DC _context;
        private readonly IConfiguration _config;
        private readonly Token _tokenService;

        public LoginController(DC context, IConfiguration config, Token tokenService)
        {
            _context = context;
            _config = config;
            _tokenService = tokenService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm][Required] string email, [FromForm][Required] string password)
        {
            var user = await Autentica(email, password);

            if (user != null)
            {
               
                HttpContext.Session.SetInt32("UserId", user.id);

                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }

            return Unauthorized("Email o password non valide.");
        }


        private async Task<Utente> Autentica(string email, string password)
        {
          
            var user = await _context.utente
                .FirstOrDefaultAsync(u => u.email.ToLower() == email.ToLower());

            if (user != null && VerificaPassword(password, user.password))
            {
                return user;
            }

            return null;
        }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerificaPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
