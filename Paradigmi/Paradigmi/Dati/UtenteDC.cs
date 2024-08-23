using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Paradigmi.Classi;
namespace Paradigmi.Dati
{
    public class UtenteDC
    {
        private readonly DC _context;

        public UtenteDC(DC context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utente>> GetUtenteAsync()
        {
           
                return await _context.utente
                    .Include(u => u.bookings) 
                    .ToListAsync();
            
         
        }

        public async Task<Utente> GetUtenteEmailAsync(String mail)
        {
            try
            {
                return await _context.utente
                    .Include(u => u.bookings) 
                    .FirstOrDefaultAsync(u => u.email == mail);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Errore nella ricerca dell'utente tramite mail.", ex);
            }
        }

        public async Task<Utente> CreaUtenteAsync(Utente user)
        {
            try
            {
                
                user.password = HashPassword(user.password);
                _context.utente.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella creazione dell'utente.", ex);
            }
        }

        public async Task<Utente> AutenticaUtenteAsync(string email, string password)
        {
            try
            {
                var user = await _context.utente.FirstOrDefaultAsync(u => u.email == email) ;
                if (user != null && ControllaPassword(password, user.password))
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
            
                throw new Exception("Errore nell'autenticazione", ex);
            }
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

        private bool ControllaPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
