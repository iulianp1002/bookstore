using BookstoreAPI.DomainModels;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BookstoreAPI.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IConfiguration _config;

        //private const string SPGetAllByBook = "dbo.sp_getAllAuthorsByBook";
        
        public AuthorRepository(IConfiguration config) 
        {
            _config = config;
        }

        public async Task<int> CreateAsync(Author author)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var result = await connection.ExecuteAsync("insert into Authors(Name) Values(@Name)", param: new {Name = author.Name}, commandType:CommandType.Text);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("delete from Authors where id=@id", new {id = id});
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var authors = await connection.QueryAsync<Author>("Select * from Authors");

            return authors.ToList();
        }

        //public async Task<List<Author>> GetAllByBookAsync(int bookId)
        //{
        //    using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        //    var authors = await connection.QueryAsync<Author>("Select * from Author where Id= @Id", new {bookId);

        //    return authors.ToList();
        //}

        public async Task<Author> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var author = await connection.QueryFirstAsync<Author>("Select * from Authors where Id=@id", new { id = id });

            return author;
        }

        public async Task<Author> GetByNameAsync(string name)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var author = await connection.QueryFirstAsync<Author>("Select * from Authors where Name like '%@Name%'", new { Name = name });

            return author;
        }

        public async Task<int> UpdateAsync(Author author)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var updatedAuthor =await connection.ExecuteAsync("Update Authors Set Name=@Name where id =@id",param: new {Name= author.Name, id = author.Id }, commandType: CommandType.Text);

            return updatedAuthor;
        }
    }
}
