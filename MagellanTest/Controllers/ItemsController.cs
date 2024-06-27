using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace MagellanTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly string _connectionString;

        public ItemsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemCreateDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand(
                    "INSERT INTO item (item_name, parent_item, cost, req_date) VALUES (@itemName, @parentItem, @cost, @reqDate) RETURNING id",
                    connection);

                command.Parameters.AddWithValue("itemName", itemDto.ItemName);
                command.Parameters.AddWithValue("parentItem", (object)itemDto.ParentItem ?? DBNull.Value);
                command.Parameters.AddWithValue("cost", itemDto.Cost);
                command.Parameters.AddWithValue("reqDate", itemDto.ReqDate);

                var id = await command.ExecuteScalarAsync();

                return Ok(new { id });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while creating the item.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand(
                    "SELECT id, item_name, parent_item, cost, req_date FROM item WHERE id = @id",
                    connection);
                command.Parameters.AddWithValue("id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var item = new ItemDto
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
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while retrieving the item.");
            }
        }

        [HttpGet("total-cost")]
        public async Task<IActionResult> GetTotalCost([FromQuery] string itemName)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT Get_Total_Cost(@itemName)", connection);
                command.Parameters.AddWithValue("itemName", itemName);

                var result = await command.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    return Ok(new { TotalCost = (decimal)result });
                }
                else
                {
                    return NotFound("Item not found or total cost could not be calculated.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while calculating the total cost.");
            }
        }
    }

    public class ItemCreateDto
    {
        public string ItemName { get; set; }
        public string? ParentItem { get; set; }
        public decimal Cost { get; set; }
        public DateTime ReqDate { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string? ParentItem { get; set; }
        public decimal Cost { get; set; }
        public DateTime ReqDate { get; set; }
    }
}
