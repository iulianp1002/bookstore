using Bookstore.Models;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Bookstore.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;

        private const string SPAddBook = "dbo.sp_addBook";
        private const string SPUpdateBook = "dbo.sp_updateBook";
        private const string SPGetAllByAuthor = "dbo.sp_getAllByAuthor";

        public BookRepository(IConfiguration config)
        {
            _config = config;
        }
        public int Create(Book book)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var result = connection.Execute(SPAddBook, book);
            return result;
        }

        public int Delete(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.Execute("delete from Books where id=@id", id);
        }

        public List<Book> GetAllBooks()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var books = connection.Query<Book>("Select * from Books");
            
            return books.ToList();
        }

        public Book GetById(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var book =  connection.QueryFirst<Book>("Select * from Books where Id=@id", new { id = id });

            return book;
        }

        public Book GetByTitle(string nameTitle)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var book = connection.QueryFirst<Book>("Select * from Books where Title like '%@title%'", new { title = nameTitle });

            return book;
        }

        public int Update(Book book)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var updatedBook = connection.Execute(SPUpdateBook, book);

            return updatedBook;
        }

        public List<Book> GetAllByAuthor(int authorId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var books = connection.Query<Book>(SPGetAllByAuthor, authorId);

            return books.ToList();
        }
    }
}
