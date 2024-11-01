using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(Client client) : ControllerBase
    {
        private readonly Client _client = client;

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            ModeledResponse<User> response = await _client.From<User>().Insert(new User
            {
                Email = "test@test.com",
                Name = "test",
            });
            
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            ModeledResponse<User> response = await _client.From<User>().Where(x => x.Name == name).Get();
            return Ok(response.Models.FirstOrDefault());
        }
    }

    [Table("Users")]
    class User : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
    }
}
