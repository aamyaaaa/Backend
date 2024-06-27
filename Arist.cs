namespace art_gallery_api.Models
{
    public partial class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FamousWork { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
