using BookstoreAPI.DomainModels;
using BookstoreAPI.Models;

namespace BookstoreAPI.Service
{
    public interface IBookService
    {
        public Task<List<Book>> BookListByAuthorAsync(string authorName);
        public Task<List<Book>> BookListAsync();
        public Task<List<BookWithAuthorsModel>> BookListWithAuthorsAsync();
        public Task<Book?> FindByIdAsync(int id);
        public Task<Book?> FindByTitleAsync(string title);
        //public Task<int> SaveAsync(Book book, List<int> authorIds);
        public Task<int> AddBookAsync(AddBookModel model);
        public Task<int> UpdateBookAsync(AddBookModel model);
        public Task<int> DeleteAsync(int id);
    }
}
