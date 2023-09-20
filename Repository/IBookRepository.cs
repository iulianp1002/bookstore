using Bookstore.Models;

namespace Bookstore.Repository
{
    public interface IBookRepository
    {
        public List<Book> GetAllBooks();
        public Book GetById(int id);
        public Book GetByTitle(string nameTitle);

        public int Create(Book book);
        public int Update(Book book);
        public int Delete(int id);
        public List<Book> GetAllByAuthor(int authorId);
    }
}
