using BookstoreAPI.DomainModels;

namespace BookstoreAPI.Repository
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetAllBooksAsync();
        public Task<Book> GetByIdAsync(int id);
        public Task<Book> GetByTitleAsync(string nameTitle);

        public Task<int> CreateAsync(Book book);
        public Task<int> UpdateAsync(Book book);
        public Task<int> DeleteAsync(int id);
       
     
    }
}
