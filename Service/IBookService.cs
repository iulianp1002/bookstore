using Bookstore.Models;

namespace Bookstore.Service
{
    public interface IBookService
    {
        List<Book> BookListByAuthor { get; }
        List<Book> BookList { get; }
        int Save(Book book);
        int Delete(int id);
    }
}
