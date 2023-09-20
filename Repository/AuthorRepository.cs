using Bookstore.Models;
using Dapper;
using System.Data.SqlClient;

namespace Bookstore.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IConfiguration _config;

        public AuthorRepository(IConfiguration config) 
        {
            _config = config;
        }

        public int Create(Author author)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var result = connection.Execute("insert into Authors(Name) Values(@Name", author);
            return result;
        }

        public int Delete(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.Execute("delete from Authors where id=@id", id);
        }

        public List<Author> GetAllAuthors()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var authors = connection.Query<Author>("Select * from Authors");

            return authors.ToList();
        }

        public Author GetById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var author = connection.QueryFirst<Author>("Select * from Authors where Id=@id", new { id = id });

            return author;
        }

        public Author GetByName(string name)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var author = connection.QueryFirst<Author>("Select * from Authors where Name like '%@Name%'", new { Name = name });

            return author;
        }

        public int Update(Author author)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var updatedAuthor = connection.Execute("Update Authors Set Name=@Name where id =@id", author);

            return updatedAuthor;
        }
    }
}
