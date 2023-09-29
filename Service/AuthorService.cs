using BookstoreAPI.DomainModels;
using BookstoreAPI.Repository;

namespace BookstoreAPI.Service
{
    public class AuthorService : IAuthorService
    {
        
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookAuthorsRepository _bookAuthorsRepository;

        public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository, IBookAuthorsRepository bookAuthorsRepository )
        {
            
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _bookAuthorsRepository = bookAuthorsRepository;
        }

        public async Task<List<Author>> AuthorListAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }

        public async Task<List<Author>> AuthorListByBookAsync(string title)
        {
            var book = await _bookRepository.GetByTitleAsync(title);
            var bookAuthors =  await _bookAuthorsRepository.GetAllByBookIdAsync(book.Id);
            var authorIds = bookAuthors.Select(x => x.AuthorId).ToList();
            var authors = await _authorRepository.GetAllAuthorsAsync();

            return  authors.Where(x=>authorIds.Contains(x.Id)).ToList();

        }

        public async Task<int> DeleteAsync(int id)
        {
          return  await _authorRepository.DeleteAsync(id);
        }

        public async Task<Author?> FindByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            
            if (author == null) return null;
            else return author;
        }

        public async Task<Author?> FindByNameAsync(string name)
        {
            var author = await _authorRepository.GetByNameAsync(name);

            if (author == null) return null;
            else return author;
        }

        public async Task<int> SaveAsync(Author author)
        {
            if (author != null && author.Id == 0)
            {
                return await _authorRepository.CreateAsync(author);
            }
            else
            {
                return await _authorRepository.UpdateAsync(author ?? new Author());
            }
        }
    }
}
