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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utente>>> GetAllUsers()
        {
            try
            {
                var users = await utenteDC.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Utente>> CreateUser([FromBody] Utente user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUsers = await utenteDC.GetUsersAsync();
                if (existingUsers.Any(u => u.getEmail() == user.getEmail()))
                {
                    return Conflict("Email gia' registrata.");
                }

              
                var createdUser = await utenteDC.CreateUserAsync(user);
                var token = this.token.GenerateToken(createdUser);

                return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.getId() }, new { createdUser, token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Utente>> GetUserById(int userId)
        {
            try
            {
                var user = await utenteDC.GetUserByIdAsync(userId);

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

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] Utente user)
        {
            if (userId != user.getId())
            {
                return BadRequest("User ID mismatch");
            }

            try
            {
                var existingUser = await utenteDC.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound();
                }

           
                existingUser.setEmail( user.getEmail());
                existingUser.setNome(user.getNome());
                existingUser.setcognome(user.getcognome());
                existingUser.setPassword(user.getPassword()); 

                await utenteDC.UpdateUserAsync(existingUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await utenteDC.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                await utenteDC.DeleteUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
