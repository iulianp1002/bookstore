using BookstoreAPI.DomainModels;

namespace BookstoreAPI.Service
{
    public interface IAuthorService
    {
        public Task<List<Author>> AuthorListByBookAsync(string bookName);
        public Task<List<Author>> AuthorListAsync();
        public Task<Author?> FindByIdAsync(int id);
        public Task<Author?> FindByNameAsync(string name);
        public Task<int> AddAuthorAsync(Author author);
        public Task<int> UpdateAuthorAsync(Author author);
        public Task<int> DeleteAsync(int id);
    }
}
