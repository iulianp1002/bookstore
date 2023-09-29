using BookstoreAPI.DomainModels;

namespace BookstoreAPI.Repository
{
    public interface IBookAuthorsRepository
    {
        public Task<List<BookAuthors>> GetAllAsync();
        public Task<BookAuthors> GetByIdAsync(int id);
        public Task<List<BookAuthors>> GetAllByBookIdAsync(int bookId);
        public Task<List<BookAuthors>> GetAllByAuthorIdAsync(int authorId);

        public Task<int> CreateAsync(int bookId, List<int> authorIds);
        public Task<int> UpdateAsync(int bookId, List<int> authorIds);
        public Task<int> DeleteAsync(int id);
        public Task<int> DeleteByBookId(int bookId);
        public Task<int> DeleteByAuthorId(int authorId);
        
    }
}
