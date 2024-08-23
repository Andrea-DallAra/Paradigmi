using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigmi.Classi;
using Paradigmi.TokenService;
using Paradigmi.Dati;
using Microsoft.AspNetCore.Authorization;

namespace Paradigmi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrazioneController : ControllerBase
    {
        private readonly UtenteDC utenteDC;
        private readonly Token token;

        public RegistrazioneController(UtenteDC _utenteDC, Token _token)
        {
            utenteDC = _utenteDC;
            token = _token;
        }

       

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Utente>> CreateUser(String Nome, String Cognome, String Email, String Password)
        {
            Utente user = new Utente(Nome,Cognome, Email, Password);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUsers = await utenteDC.GetUsersAsync();
                if (existingUsers.Any(u => u.email == user.email))
                {
                    return Conflict("Email gia' registrata.");
                }

              
                var createdUser = await utenteDC.CreateUserAsync(user);
                var token = this.token.GenerateToken(createdUser);

                return CreatedAtAction(nameof(GetUserByEmail), new { userId = createdUser.id }, new { createdUser, token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Get User")]
        public async Task<ActionResult<Utente>> GetUserByEmail(String Email)
        {
            try
            {
                var user = await utenteDC.GetUserByEmailAsync(Email);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
      
    }
}
