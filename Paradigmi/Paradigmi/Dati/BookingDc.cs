using Microsoft.EntityFrameworkCore;
using Paradigmi.Classi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigmi.Dati
{
    public class BookingDc
    {
        private readonly DC _context;

        public BookingDc(DC context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetPrenotazioniAsync()
        {
            try
            {
                return await _context.bookings.Include(b => b.risorsa).Include(b => b.utente).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la ricerca delle prenotazioni.", ex);
            }
        }

        public async Task<Booking> GetPrenotazioniIdAsync(int bookingId)
        {
            try
            {
                return await _context.bookings
                    .Include(b => b.risorsa)
                    .Include(b => b.utente)
                    .FirstOrDefaultAsync(b => b.id == bookingId);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la ricerca di ID.", ex);
            }
        }

        public async Task<Booking> PrenotaAsync(Booking booking)
        {
            try
            {
                _context.bookings.Add(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella prenotazione.", ex);
            }
        }


        public async Task<bool> ControlloPrenotazioneAsync(int resourceId, DateTime startDate, DateTime endDate)
        {
            return await _context.bookings
                .AnyAsync(b => b.idRisorsa== resourceId && (b.inizio < endDate && b.fine> startDate));
        }

        public async Task<(IEnumerable<Risorsa> AvailableResources, int TotalResults, int TotalPages)> GetAvailableResourcesAsync(DateTime inizio, DateTime fine, string resourceType, int page, int pageSize)
        {
            
            var bookedResourceIds = await _context.bookings
            .Where(b => (b.inizio >= inizio && b.fine <= fine) &&                 
                   (string.IsNullOrEmpty(resourceType) || b.risorsa.tipoRisorsa == resourceType)
                   )
            .Select(b => b.idRisorsa)
            .Distinct()
            .ToListAsync();


            var availableResourcesQuery = _context.risorse
                .Where(r => !bookedResourceIds.Contains(r.id) &&
                            (string.IsNullOrEmpty(resourceType) || r.tipoRisorsa == resourceType));

            var totalResources = await availableResourcesQuery.CountAsync();

            var availableResources = await availableResourcesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalResources / (double)pageSize);

            return (availableResources, totalResources, totalPages);
        }
    }
}
