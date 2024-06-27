using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Persistence;
using art_gallery_api.Models;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/artists")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistDataAccess _artistRepo;
        
        public ArtistsController(IArtistDataAccess artistRepo)
        {
            _artistRepo = artistRepo;
        }

        // Retrieve all Artists from the database
        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GetAllArtists()
        {
            var artists = _artistRepo.GetArtists();
            if (artists == null || !artists.Any())
            {
                return NotFound("No artists found.");
            }
            return Ok(artists);
        }

        // Retrieve a specific Artist by ID from the database
        [HttpGet("{id}")]
        public ActionResult<Artist> GetArtistById(int id)
        {
            var artist = _artistRepo.GetArtistById(id);
            if (artist == null)
            {
                return NotFound();
            }
            return Ok(artist);
        }

        // Add a new Artist to the database
        [HttpPost]
        public IActionResult AddArtist(Artist newArtist)
        {
            if (newArtist == null)
            {
                return BadRequest("Invalid artist data.");
            }

            _artistRepo.InsertArtist(newArtist);

            return CreatedAtAction(nameof(GetArtistById), new { id = newArtist.ArtistId }, newArtist);
        }

        // Update an existing Artist in the database
        [HttpPut("{id}")]
        public IActionResult UpdateArtist(int id, Artist updatedArtist)
        {
            var existingArtist = _artistRepo.GetArtistById(id);
            if (existingArtist == null)
            {
                return NotFound("Artist not found.");
            }

            updatedArtist.ArtistId = id;
            _artistRepo.UpdateArtist(id, updatedArtist);

            return NoContent();
        }

        // Delete an Artist from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(int id)
        {
            var artist = _artistRepo.GetArtistById(id);
            if (artist == null)
            {
                return NotFound("Artist not found.");
            }

            _artistRepo.DeleteArtist(id);
            return NoContent();
        }
    }
}
