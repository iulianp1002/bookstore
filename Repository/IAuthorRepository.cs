using Bookstore.Models;

namespace Bookstore.Repository
{
    public interface IAuthorRepository
    {
        public List<Author> GetAllAuthors();
        public Author GetById(int id);
        public Author GetByName(string name);
        
        public int Create(Author author);
        public int Update(Author book);
        public int Delete(int id);
    }
}
