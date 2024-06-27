using System.Collections.Generic;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IExhibitionTourDataAccess
    {
        List<ExhibitionTour> GetExhibitionTours();
        ExhibitionTour? GetExhibitionTourById(int id);
        void InsertExhibitionTour(ExhibitionTour newExhibitionTour);
        void UpdateExhibitionTour(int id, ExhibitionTour updatedExhibitionTour);
        void DeleteExhibitionTour(int id);
    }
}
