using Bookstore.Models;
using Bookstore.Repository;

namespace BookstoreAPI.Service
{
    public class AuthorService : IAuthorService
    {
        
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            
            _authorRepository = authorRepository;
        }

        public List<Author> AuthorList()
        {
            return _authorRepository.GetAllAuthors();
        }

        public List<Author> AuthorListByBook(string bookName)
        {
            return _authorRepository.GetAllAuthors();
        }

        public int Delete(int id)
        {
          return  _authorRepository.Delete(id);
        }

        public int Save(Author author)
        {
            if (author != null && author.Id == 0)
            {
                return _authorRepository.Create(author);
            }
            else
            {
                return _authorRepository.Update(author ?? new Author());
            }
        }
    }
}
