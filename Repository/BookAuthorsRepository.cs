using BookstoreAPI.DomainModels;
using Core.Common.Extensions;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BookstoreAPI.Repository
{
    public class BookAuthorsRepository : IBookAuthorsRepository
    {
        private readonly IConfiguration _config;

        const string SPAddBookAuthors = "dbo.sp_AddBookAuthors";
        const string SPUpdateBookAuthors = "dbo.sp_UpdateBookAuthorsOld";

        public BookAuthorsRepository(IConfiguration config)
        {
            _config = config;
        }
        public class AuthorTemplate
        {
            public int Id { get; set; }
        }
        public async Task<int> CreateAsync(int bookId, List<int> authorIds)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));


            // create and fill template
            List<AuthorTemplate> authorTemplates = new List<AuthorTemplate>();
            foreach (var author in authorIds)
            {
                authorTemplates.Add(new AuthorTemplate { Id = author });
            }


            // create DataTable
            DataTable projectsDT = new();
            projectsDT.Columns.Add(nameof(AuthorTemplate.Id), typeof(int));


            // add rows to DataTable
            foreach (var project in authorTemplates)
            {
                var row = projectsDT.NewRow();
                row[nameof(AuthorTemplate.Id)] = project.Id;

                projectsDT.Rows.Add(row);
            }

            // create parameters
            var parameters = new
            {
                BookId = bookId,
                AuthorIds = projectsDT.AsTableValuedParameter("[dbo].[AuthorIdType]")
            };

            var createdAuthor = await connection.ExecuteAsync(SPAddBookAuthors, param:parameters,commandType: CommandType.StoredProcedure);
           
            return createdAuthor;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("delete from BookAuthors where Id=@Id", new { Id = id });
        }

        public async Task<List<BookAuthors>> GetAllAsync()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var bookAuthors = await connection.QueryAsync<BookAuthors>("Select * from BookAuthors", commandType: CommandType.Text);

            return bookAuthors.ToList();
        }

        public async Task<List<BookAuthors>> GetAllByAuthorIdAsync(int authorId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var bookAuthors = await connection.QueryAsync<BookAuthors>("Select * from BookAuthors where AuthorId=@AuthorId", param: new { AuthorId = authorId }, commandType: CommandType.Text);

            return bookAuthors.ToList();
        }

        public async Task<List<BookAuthors>> GetAllByBookIdAsync(int bookId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var bookAuthors = await connection.QueryAsync<BookAuthors>("Select * from BookAuthors where BookId=@BookId",param: new { BookId = bookId }, commandType: CommandType.Text);

            return bookAuthors.ToList();
        }

        public async Task<BookAuthors> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var author = await connection.QueryFirstAsync<BookAuthors>("Select * from Authors where Id=@Id", new { Id = id });

            return author;
        }

        public async Task<int> UpdateAsync(int bookId, List<int> authorIds)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            // create and fill template
            List<AuthorTemplate> authorTemplates = new List<AuthorTemplate>();
            foreach (var author in authorIds)
            {
                authorTemplates.Add(new AuthorTemplate { Id = author });
            }


            // create DataTable
            DataTable projectsDT = new();
            projectsDT.Columns.Add(nameof(AuthorTemplate.Id), typeof(int));


            // add rows to DataTable
            foreach (var project in authorTemplates)
            {
                var row = projectsDT.NewRow();
                row[nameof(AuthorTemplate.Id)] = project.Id;

                projectsDT.Rows.Add(row);
            }

            // create parameters
            var parameters = new
            {
                BookId = bookId,
                AuthorIds = projectsDT.AsTableValuedParameter("[dbo].[AuthorIdType]")
            };

            var updatedAuthor = await connection.ExecuteAsync(SPUpdateBookAuthors, param: parameters, commandType: CommandType.StoredProcedure);

            return updatedAuthor;
        }

        public async Task<int> DeleteByBookId(int bookId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("delete from BookAuthors where BookId=@BookId", new { BookId = bookId });
        }

        public async Task<int> DeleteByAuthorId(int authorId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("delete from BookAuthors where AuthorId=@AuthorId", new { AuthorId = authorId });
        }
    }
}
