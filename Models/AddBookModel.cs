namespace BookstoreAPI.Models
{
    public class AddBookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public List<int> AuthorIds { get;set; } = new List<int>();
    }
}
