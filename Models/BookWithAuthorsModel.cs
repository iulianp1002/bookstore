namespace BookstoreAPI.Models
{
    public class BookWithAuthorsModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        
    }
}
