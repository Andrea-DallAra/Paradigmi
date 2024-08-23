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

        public async Task<IEnumerable<Risorsa>> GetRisorseAsync()
        {
            try
            {
                return await _context.risorse.ToListAsync();
            }
            catch (Exception ex)
            {
               
                throw new Exception("Errore nella ricerca delle risorse", ex);
            }
        }

        public async Task<Risorsa> GetRisorseIdAsync(int idrisorsa)
        {
            try
            {
              
                return await _context.risorse.FirstOrDefaultAsync(r => r.id == idrisorsa);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Errore nella ricerca della risorsa per id.", ex);
            }
        }

        public async Task<Risorsa> CreaRisorsaAsync(Risorsa risorsa)
        {
            
            try
            {
                _context.risorse.Add(risorsa);
                await _context.SaveChangesAsync();

                return risorsa;
            }
            catch (Exception ex)
            {
            
                throw new Exception("Errore nella creazione delle risorse.", ex);
            }
        }

  

        public async Task<IEnumerable<Risorsa>> GetRisorseDisponibiliAsync(DateTime startDate, DateTime endDate, string tipoRisorsa = null)
        {
            try
            {
                
                var query = _context.risorse.Include(r => r.booking).AsQueryable();

                
                if (!string.IsNullOrEmpty(tipoRisorsa))
                {
                    query = query.Where(r => r.tipoRisorsa == tipoRisorsa);
                }

               
                return await query
                    .Where(r => !r.booking.Any(b => b.inizio < endDate && b.fine > startDate))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la ricerca delle risorse.", ex);
            }
        }


    }
}
