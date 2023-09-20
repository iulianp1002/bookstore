using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using BookstoreAPI.Service;
using Bookstore.Models;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllPersons()
        {
            
            IEnumerable<Author> authors = await _authorService.get;
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var person = await connection.QueryFirstAsync<Person>("Select * from Persons where Id=@id", new { id = id });
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<List<Person>>> CreatePerson(Person person)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Persons(Name,Email,Phone,Rating) Values(@Name,@Email,@Phone,@Rating", person);
            return Ok(await GetPersons(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Person>>> UpdatePerson(Person person)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update Persons Set Name =@Name,Email =@Email,Phone=@Phone,Rating=@Rating where id =@id", person);
            return Ok(await GetPersons(connection));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<List<Person>>> DeletePerson(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from Persons where id=@id", id);
            return Ok(await GetPersons(connection));

        }

        #region private methods
        private static async Task<IEnumerable<Person>> GetPersons(SqlConnection connection)
        {
            return await connection.QueryAsync<Person>("Select * from Persons");
        }
        #endregion
    }
}
