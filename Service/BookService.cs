using Bookstore.Models;
using Bookstore.Repository;

namespace Bookstore.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }
        

        public int Delete(int id)
        {
           return _bookRepository.Delete(id);
        }

        public int Save(Book book)
        {
            if (book!=null && book.Id == 0)
            {
                return _bookRepository.Create(book);
            }
            else
            {
               return  _bookRepository.Update(book ?? new Book());
            }
        }

        List<Book> IBookService.BookList()
        {
            return _bookRepository.GetAllBooks();
        }

        List<Book> IBookService.BookListByAuthor(string authorName)
        {
            var author = _authorRepository.GetByName(authorName);
            return _bookRepository.GetAllByAuthor(author.Id);
        }
    }
}
