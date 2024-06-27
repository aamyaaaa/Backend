using System.Collections.Generic;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IFactDataAccess
    {
        List<Fact> GetFacts();
        Fact? GetFactById(int id);
        void InsertFact(Fact newFact);
        void UpdateFact(int id, Fact updatedFact);
        void DeleteFact(int id);
    }
}
