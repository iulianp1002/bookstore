using BookstoreAPI.DomainModels;
using BookstoreAPI.Repository;
using BookstoreAPI.Models;
using System.Linq;
using System.Text;
using System.Transactions;

namespace BookstoreAPI.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookAuthorsRepository _bookAuthorsRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookAuthorsRepository bookAuthorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _bookAuthorsRepository = bookAuthorRepository;
        }
        

        public async Task<int> DeleteAsync(int id)
        {
           return await _bookRepository.DeleteAsync(id);
        }

        public async Task<Book?> FindByIdAsync(int id)
        {
           var book = await _bookRepository.GetByIdAsync(id);
            
           if (book == null) return null;
           else return book;
        }
        public async Task<int> AddBookAsync(AddBookModel model)
        {
            var result = -1;
            Book book = new Book();

            if (model != null)
            {
                book.Title = model.Title;
                book.Description = model.Description;
                book.PictureUrl = model.PictureUrl;

                //using (var scope = new TransactionScope())
                //{
                    var bookId = await _bookRepository.CreateAsync(book);
                    result = await _bookAuthorsRepository.CreateAsync(bookId, model.AuthorIds);

                //    scope.Complete();
                //}
            }

            return result;
        }


        public async Task<int> UpdateBookAsync(AddBookModel model)
        {
            var result = -1;
            Book book = new Book();

            if (model != null)
            {
                book.Id = model.Id;
                book.Title = model.Title;
                book.Description = model.Description;
                book.PictureUrl = model.PictureUrl;

                //using (var scope = new TransactionScope())
                //{
                    var bookId = await _bookRepository.UpdateAsync(book);
                    result = await _bookAuthorsRepository.UpdateAsync(book.Id, model.AuthorIds);

                   // scope.Complete();
                //}
            }

            return result;
        }
        

        public async Task<List<Book>> BookListAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<List<Book>> BookListByAuthorAsync(string authorName)
        {
            var author = await _authorRepository.GetByNameAsync(authorName);
            var booksByAuthor = await _bookAuthorsRepository.GetAllByAuthorIdAsync(author.Id);
            var bookIds = booksByAuthor.Select(book => book.Id).ToList();
            var books =  _bookRepository.GetAllBooksAsync().Result.Where(book=>bookIds.Contains(book.Id));

            return books.ToList();
        }

        public async Task<List<BookWithAuthorsModel>> BookListWithAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAuthorsAsync();
            var books = await _bookRepository.GetAllBooksAsync();
            var bookAuthors = await _bookAuthorsRepository.GetAllAsync();
            var result = new List<BookWithAuthorsModel>();

            foreach (var book in books)
            {
                var bookWithAuthorModel = new BookWithAuthorsModel();
                bookWithAuthorModel.Title = book.Title;
                bookWithAuthorModel.Description = book.Description;
                bookWithAuthorModel.PictureUrl = book.PictureUrl;

                //get authors for a book
                var bookAuthorsForBook = await _bookAuthorsRepository.GetAllByBookIdAsync(book.Id);
                var authorIdsForBook = bookAuthorsForBook.Select(x => x.AuthorId);
                var authorsForBook = authors.Where(author => authorIdsForBook.Contains(author.Id)).ToList() ;
                bookWithAuthorModel.Authors = ConcatNames(authorsForBook);

                result.Add(bookWithAuthorModel);
            }
            return result;
        }

        public async Task<Book?> FindByTitleAsync(string title)
        {
            var book = await _bookRepository.GetByTitleAsync(title);

            if (book == null) return null;
            else return book;
        }

        #region private Methods

        private string ConcatNames(List<Author> authors)
        {
            var result = new StringBuilder();

            foreach (var author in authors)
            {
                result.Append(author.Name + " ");
            }

            return result.ToString();
        }

        #endregion
    }
}
