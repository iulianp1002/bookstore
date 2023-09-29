using BookstoreAPI.DomainModels;

namespace BookstoreAPI.Repository
{
    public interface IAuthorRepository
    {
        public Task<List<Author>> GetAllAuthorsAsync();
        public Task<Author> GetByIdAsync(int id);
        public Task<Author> GetByNameAsync(string name);
        
        public Task<int> CreateAsync(Author author);
        public Task<int> UpdateAsync(Author book);
        public Task<int> DeleteAsync(int id);
        
    }
}
