using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Persistence;
using art_gallery_api.Models;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/exhibitiontours")]
    public class ExhibitionToursController : ControllerBase
    {
        private readonly IExhibitionTourDataAccess _tourRepo;

        public ExhibitionToursController(IExhibitionTourDataAccess tourRepo)
        {
            _tourRepo = tourRepo;
        }

        // Retrieve all Exhibition Tours from the database
        [HttpGet]
        public ActionResult<IEnumerable<ExhibitionTour>> GetAllExhibitionTours()
        {
            var tours = _tourRepo.GetExhibitionTours();
            if (tours == null || !tours.Any())
            {
                return NotFound("No exhibition tours found.");
            }
            return Ok(tours);
        }

        // Retrieve a specific Exhibition Tour by ID from the database
        [HttpGet("{id}")]
        public ActionResult<ExhibitionTour> GetExhibitionTourById(int id)
        {
            var tour = _tourRepo.GetExhibitionTourById(id);
            if (tour == null)
            {
                return NotFound();
            }
            return Ok(tour);
        }

        // Add a new Exhibition Tour to the database
        [HttpPost]
        public IActionResult AddExhibitionTour(ExhibitionTour newTour)
        {
            if (newTour == null)
            {
                return BadRequest("Invalid exhibition tour data.");
            }

            _tourRepo.InsertExhibitionTour(newTour);

            return CreatedAtAction(nameof(GetExhibitionTourById), new { id = newTour.TourId }, newTour);
        }

        // Update an existing Exhibition Tour in the database
        [HttpPut("{id}")]
        public IActionResult UpdateExhibitionTour(int id, ExhibitionTour updatedTour)
        {
            var existingTour = _tourRepo.GetExhibitionTourById(id);
            if (existingTour == null)
            {
                return NotFound("Exhibition tour not found.");
            }

            updatedTour.TourId = id;
            _tourRepo.UpdateExhibitionTour(id, updatedTour);

            return NoContent();
        }

        // Delete an Exhibition Tour from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteExhibitionTour(int id)
        {
            var tour = _tourRepo.GetExhibitionTourById(id);
            if (tour == null)
            {
                return NotFound("Exhibition tour not found.");
            }

            _tourRepo.DeleteExhibitionTour(id);
            return NoContent();
        }
    }
}
