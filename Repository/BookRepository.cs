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
        public BookRepository(IConfiguration config)
        {
            _config = config;
        }
        public int Create(Book book)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var result = connection.Execute("insert into Books(Title,Description,PictureUrl) Values(@Title,@Description,@PictureUrl", book);
            return result;
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
