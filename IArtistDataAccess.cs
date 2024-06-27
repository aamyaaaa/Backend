using System.Collections.Generic;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IArtistDataAccess
    {
        List<Artist> GetArtists();
        Artist? GetArtistById(int id);
        void InsertArtist(Artist newArtist);
        void UpdateArtist(int id, Artist updatedArtist);
        void DeleteArtist(int id);
    }
}
