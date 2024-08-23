using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigmi.Classi;
using Paradigmi.Dati;
using System.Linq;

namespace Paradigmi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RisorsaController : ControllerBase
    {
        private readonly RisorsaDC _risorsaDC;

        public RisorsaController(RisorsaDC _RisDC)
        {
            _risorsaDC = _RisDC;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Get Risorsa")]
        public async Task<ActionResult<Risorsa>> GetResource(int id)
        {
            try
            {
                var resource = await _risorsaDC.GetRisorseIdAsync(id);

                if (resource == null)
                {
                    return NotFound("Risorsa non trovata.");
                }

                return Ok(resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Crea Risorsa")]
        public async Task<ActionResult<Risorsa>> AddResource([FromQuery] string nome, [FromQuery] string tipoRisorsa)
        {
            
            Risorsa risorsa = new Risorsa(nome, tipoRisorsa);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               

                var createdResource = await _risorsaDC.CreaRisorsaAsync(risorsa);

                return CreatedAtAction(nameof(GetResource), new { id = createdResource.id }, createdResource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    

       
    }
}
