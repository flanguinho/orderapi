using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("Create")]
        public IActionResult Create(Dictionary<string, string> body)
        {
            try
            {
                string connString = _configuration.GetSection("MySQLConnectionString").Value;
                MySqlConnection conn = new MySqlConnection(connString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO orders(source,destiny) VALUES(@source, @destiny)";
                comm.Parameters.AddWithValue("@source", body["source"]);
                comm.Parameters.AddWithValue("@destiny", body["destiny"]);
                comm.ExecuteNonQuery();
                conn.Close();

                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
        [HttpGet("Health")]
        public IActionResult Health()
        {
            return Ok("running...");
        }
    }
}
