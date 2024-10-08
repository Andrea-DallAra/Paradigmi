﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigmi.Classi;
using Paradigmi.Dati;
using Microsoft.AspNetCore.Http;

namespace Paradigmi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingDc _bookingDC;

        public BookingController(BookingDc bookingRepository)
        {
            _bookingDC = bookingRepository;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                var bookings = await _bookingDC.GetPrenotazioniAsync();
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
                var booking = await _bookingDC.GetPrenotazioniIdAsync(id);

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
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized("Devi prima effettuare il login.");
            }

            Booking booking = new Booking(Inizio, Fine)
            {
                idRisorsa = Codice,
                idUtente = userId.Value 
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isBooked = await _bookingDC.ControlloPrenotazioneAsync(booking.idRisorsa, booking.inizio, booking.fine);

                if (isBooked)
                {
                    return BadRequest("La prenotazione non e' disponibile per quella data.");
                }

                var createdBooking = await _bookingDC.PrenotaAsync(booking);

                return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.id }, createdBooking);
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

                var (availableResources, totalResults, totalPages) = await _bookingDC.GetAvailableResourcesAsync(dataInizio, dataFine, codiceRisorsa, page, pageSize);

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
