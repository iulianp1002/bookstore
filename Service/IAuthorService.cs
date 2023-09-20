using Bookstore.Models;

namespace BookstoreAPI.Service
{
    public interface IAuthorService
    {
        List<Author> AuthorListByBook(string bookName);
        List<Author> AuthorList();

        int Save(Author author);
        int Delete(int id);
    }
}
