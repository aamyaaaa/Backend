namespace art_gallery_api.Models
{
    public partial class ExhibitionTour
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
