using Bookstore.Models;

namespace Bookstore.Service
{
    public interface IBookService
    {
        List<Book> BookListByAuthor(string authorName);
        List<Book> BookList();

        int Save(Book book);
        int Delete(int id);
    }
}
