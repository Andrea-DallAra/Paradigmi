using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Paradigmi.Classi;

namespace Paradigmi.Dati
{
    public class RisorsaDC
    {
        private readonly DC _context;

        public RisorsaDC(DC context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Risorsa>> GetResourcesAsync()
        {
            try
            {
                return await _context.risorse.ToListAsync();
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error fetching resources.", ex);
            }
        }

        public async Task<Risorsa> GetResourceByIdAsync(int idrisorsa)
        {
            try
            {
              
                return await _context.risorse.FirstOrDefaultAsync(r => r.GetId() == idrisorsa);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error fetching resource by ID.", ex);
            }
        }

        public async Task<Risorsa> CreateResourceAsync(Risorsa risorsa)
        {
            try
            {
                _context.risorse.Add(risorsa);
                await _context.SaveChangesAsync();

                return risorsa;
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error creating resource.", ex);
            }
        }

        public async Task<Risorsa> UpdateResourceAsync(Risorsa risorsa)
        {
            try
            {
                _context.risorse.Update(risorsa);
                await _context.SaveChangesAsync();
                return risorsa;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error updating resource.", ex);
            }
        }

        public async Task<bool> DeleteResourceAsync(int idrisorsa)
        {
            try
            {
                var risorsa = await _context.risorse.FindAsync(idrisorsa);
                if (risorsa == null)
                {
                    return false;
                }

                _context.risorse.Remove(risorsa);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error deleting resource.", ex);
            }
        }

        public async Task<IEnumerable<Risorsa>> GetAvailableResourcesAsync(DateTime startDate, DateTime endDate, string tipoRisorsa = null)
        {
            try
            {
                
                var query = _context.risorse.Include(r => r.GetBooking()).AsQueryable();

                
                if (!string.IsNullOrEmpty(tipoRisorsa))
                {
                    query = query.Where(r => r.GetTipoRisorsa() == tipoRisorsa);
                }

               
                return await query
                    .Where(r => !r.GetBooking().Any(b => b.GetInizio() < endDate && b.GetFine() > startDate))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching available resources.", ex);
            }
        }


    }
}
