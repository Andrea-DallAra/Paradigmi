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
        private readonly RisorsaDC _resourceRepository;

        public RisorsaController(RisorsaDC resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Risorsa>>> GetResources()
        {
            try
            {
                var resources = await _resourceRepository.GetResourcesAsync();
                return Ok(resources);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Risorsa>> GetResource(int id)
        {
            try
            {
                var resource = await _resourceRepository.GetResourceByIdAsync(id);

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

        [HttpPost]
        public async Task<ActionResult<Risorsa>> AddResource([FromBody] Risorsa risorsa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdResource = await _resourceRepository.CreateResourceAsync(risorsa);

                return CreatedAtAction(nameof(GetResource), new { id = createdResource.GetId() }, createdResource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Risorsa>> UpdateResource([FromBody] Risorsa risorsa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingResource = await _resourceRepository.GetResourceByIdAsync(risorsa.GetId());

                if (existingResource == null)
                {
                    return NotFound("Risorsa non trovata.");
                }

                var updatedResource = await _resourceRepository.UpdateResourceAsync(risorsa);

                return Ok(updatedResource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            try
            {
                var success = await _resourceRepository.DeleteResourceAsync(id);

                if (!success)
                {
                    return NotFound("Risorsa non trovata.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Disponibilita'")]
        public async Task<ActionResult> SearchAvailability([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string tipoRisorsa = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
               
                if (startDate >= endDate)
                {
                    return BadRequest("StartDate deve essere precedente a EndDate.");
                }

                var availableResources = await _resourceRepository.GetAvailableResourcesAsync(startDate, endDate, tipoRisorsa);

                var result = new
                {
                    TotalResults = availableResources.Count(),
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
