using BookstoreAPI.DomainModels;
using Core.Common.Extensions;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace BookstoreAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;

        private const string SPAddBookModel = "dbo.sp_AddBookWithId";
        public BookRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<int> CreateAsync(Book book)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var result = await connection.QueryAsync<int>(SPAddBookModel, new
            {
                Title = book.Title,
                Description = book.Description,
                PictureUrl = book.PictureUrl,
            }, commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("Delete from Books where Id=@Id", new { Id = id });
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var books = await connection.QueryAsync<Book>("Select * from Books");
            
            return books.ToList();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var book = await connection.QueryFirstAsync<Book>("Select * from Books where Id=@Id", new { Id = id });

            return book;
        }

        public async Task<Book> GetByTitleAsync(string nameTitle)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var book = await connection.QueryFirstAsync<Book>("Select * from Books where Title like '%@title%'", new { title = nameTitle });

            return book;
        }

        public async Task<int> UpdateAsync(Book book)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var updatedBook = await connection.ExecuteAsync("Update Books Set Title = @Title, Description = @Description, PictureUrl = @PictureUrl where Id=@Id", new
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PictureUrl = book.PictureUrl,
            }, commandType: CommandType.Text);

            return updatedBook;
        }

        //public async Task<List<Book>> GetAllByAuthorAsync(int authorId)
        //{
        //    using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        //    var books = await connection.QueryAsync<Book>(SPGetAllByAuthor, authorId);

        //    return books.ToList();
        //}

        //public async Task<List<Book>> GetBooksWithAuthorsAsync()
        //{
            
        //    using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        //    var books = await connection.QueryAsync<Book>(SPGetAllBooksWithAuthors);

        //    return books.ToList();
            
        //}
    }
}
