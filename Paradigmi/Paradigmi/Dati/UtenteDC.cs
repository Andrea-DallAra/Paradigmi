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

        public async Task<IEnumerable<Utente>> GetUsersAsync()
        {
            try
            {
                return await _context.utenti
                    .Include(u => u.getBookings()) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
        
                throw new Exception("Error fetching users.", ex);
            }
        }

        public async Task<Utente> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _context.utenti
                    .Include(u => u.getBookings()) 
                    .FirstOrDefaultAsync(u => u.getId() == userId);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error fetching user by ID.", ex);
            }
        }

        public async Task<Utente> CreateUserAsync(Utente user)
        {
            try
            {
                
                user.setPassword(HashPassword(user.getPassword()));
                _context.utenti.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user.", ex);
            }
        }

        public async Task<Utente> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.utenti.FirstOrDefaultAsync(u => u.getEmail() == email) ;
                if (user != null && VerifyPassword(password, user.getPassword()))
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error authenticating user.", ex);
            }
        }

        public async Task UpdateUserAsync(Utente user)
        {
            try
            {
                
                user.setPassword(HashPassword(user.getPassword()));
                _context.utenti.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error updating user.", ex);
            }
        }

        public async Task DeleteUserAsync(Utente user)
        {
            try
            {
                _context.utenti.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error deleting user.", ex);
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

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
