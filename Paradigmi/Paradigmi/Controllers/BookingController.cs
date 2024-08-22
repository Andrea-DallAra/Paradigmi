using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigmi.Classi;
using Paradigmi.Dati;

namespace Paradigmi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingDc _bookingRepository;

        public BookingController(BookingDc bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                var bookings = await _bookingRepository.GetBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("id")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(id);

                if (booking == null)
                {
                    return NotFound("Prenotazione non trovata.");
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Prenota")]
        public async Task<ActionResult<Booking>> Prenota([FromQuery] DateTime Inizio, [FromQuery] DateTime Fine, [FromQuery] int Codice)
        {
            Booking booking = new Booking(Inizio, Fine);
            booking.SetIdRisorsa(Codice);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                var isBooked = await _bookingRepository.IsResourceBookedAsync(booking.GetIdRisorsa(), booking.GetInizio(), booking.GetFine());

                if (isBooked)
                {
                    return BadRequest("La prenotazione non e' disponibile per quella data.");
                }

                var createdBooking = await _bookingRepository.CreateBookingAsync(booking);

                return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.GetId() }, createdBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      


        [HttpGet("disponibilita'")]
        public async Task<ActionResult> SearchAvailability([FromQuery] DateTime dataInizio, [FromQuery] DateTime dataFine,  [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string codiceRisorsa = null)
        {
            try
            {
              
                if (dataInizio >= dataFine)
                {
                    return BadRequest("DataInizio deve essere precedente a DataFine.");
                }

                var (availableResources, totalResults, totalPages) = await _bookingRepository.GetAvailableResourcesAsync(dataInizio, dataFine, codiceRisorsa, page, pageSize);

                var result = new
                {
                    TotalResults = totalResults,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    Resources = availableResources
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
