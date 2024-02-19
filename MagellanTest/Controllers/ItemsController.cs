using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using Npgsql;

namespace MagellanTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly string _connectionstring;
        private string? _connectionString;

        public ItemsController()
        {
            _connectionstring = "Postgresql Connection string";

        }

        [HttpPost]
        public IActionResult CreateItem([FromBody] Item item)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO item (item_name, parent_item, cost, req_date) VALUES (@itemName, @parentItem, @cost, @reqDate) RETURNING id";
                        command.Parameters.AddWithValue("itemName", item.ItemName);
                        command.Parameters.AddWithValue("parentItem", item.ParentItem);
                        command.Parameters.AddWithValue("cost", item.Cost);
                        command.Parameters.AddWithValue("reqDate", item.ReqDate);

                        var id = (long)command.ExecuteScalar();

                        return Ok(new { id });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT id, item_name, parent_item, cost, req_date FROM item WHERE id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var item = new
                    {
                        Id = reader.GetInt32(0),
                        ItemName = reader.GetString(1),
                        ParentItem = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Cost = reader.GetDecimal(3),
                        ReqDate = reader.GetDateTime(4)
                    };
                    return Ok(item);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (NpgsqlException ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("total-cost")]
        public IActionResult GetTotalCost(string itemName)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("Get_Total_Cost", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("item_name", itemName);

                    try
                    {
                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Ok(result); 
                        }
                        else
                        {
                            return NotFound(); 
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        
                        return StatusCode(500, $"Database error: {ex.Message}");
                    }
                }
            }
        }
    }
    public class Item
    {

        public string ItemName { get; set; }
        public string ParentItem { get; set; }
        public decimal Cost { get; set; }
        public DateTime ReqDate { get; set; }


    }
}
