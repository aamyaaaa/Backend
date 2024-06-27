using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Persistence;
using art_gallery_api.Models;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/facts")]
    public class FactsController : ControllerBase
    {
        private readonly IFactDataAccess _factRepo;

        public FactsController(IFactDataAccess factRepo)
        {
            _factRepo = factRepo;
        }

        // Retrieve all Facts from the database
        [HttpGet]
        public ActionResult<IEnumerable<Fact>> GetAllFacts()
        {
            var facts = _factRepo.GetFacts();
            if (facts == null || !facts.Any())
            {
                return NotFound("No facts found.");
            }
            return Ok(facts);
        }

        // Retrieve a specific Fact by ID from the database
        [HttpGet("{id}")]
        public ActionResult<Fact> GetFactById(int id)
        {
            var fact = _factRepo.GetFactById(id);
            if (fact == null)
            {
                return NotFound();
            }
            return Ok(fact);
        }

        // Add a new Fact to the database
        [HttpPost]
        public IActionResult AddFact(Fact newFact)
        {
            if (newFact == null)
            {
                return BadRequest("Invalid fact data.");
            }

            _factRepo.InsertFact(newFact);

            return CreatedAtAction(nameof(GetFactById), new { id = newFact.FactId }, newFact);
        }

        // Update an existing Fact in the database
        [HttpPut("{id}")]
        public IActionResult UpdateFact(int id, Fact updatedFact)
        {
            var existingFact = _factRepo.GetFactById(id);
            if (existingFact == null)
            {
                return NotFound("Fact not found.");
            }

            updatedFact.FactId = id;
            _factRepo.UpdateFact(id, updatedFact);

            return NoContent();
        }

        // Delete a Fact from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteFact(int id)
        {
            var fact = _factRepo.GetFactById(id);
            if (fact == null)
            {
                return NotFound("Fact not found.");
            }

            _factRepo.DeleteFact(id);
            return NoContent();
        }
    }
}
