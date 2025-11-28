using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventorySrv.Dtos;
using InventorySrv.Models;
using Shared.Models;
using InventorySrv.Services;

namespace InventorySrv.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _InventoryService;
        private readonly ILogger _logger;

        public InventoryController(IInventoryService InventoryService, ILogger<InventoryController> logger)
        {
            _InventoryService = InventoryService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventorys(Guid id)
        {
            try
            {
                var item = await _InventoryService.GetInventoryAsync(id);

                if (item == null)
                    return NotFound($"Inventory with ID {id} not found.");

                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching inventory by ID");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<InventoryItem>>> GetUserInventorys(
            [FromQuery] InventoryFilterDto filter,
            [FromQuery] int? start = null,
            [FromQuery] int? end = null)
        {
            try
            {
                var InventoryDtos = await _InventoryService.GetInventorysAsync(filter, start, end);

                return Ok(InventoryDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching user Inventorys");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItem>> CreateInventoryForUser(
                                [FromBody] InventoryItem inventory)
        {
            
            try
            {
                var createdInventory = await _InventoryService.CreateInventoryForUserAsync(inventory);
                return Created("", createdInventory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating Inventory for user");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<InventoryItem>> UpdateInventoryForUser(
                                [FromBody] InventoryItem inventory)
        {

            try
            {
                var updatedInventory = await _InventoryService.UpdateInventoryForUserAsync(inventory);
                return Ok(updatedInventory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating Inventory for user");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryItem>> DeleteInventoryForUser(Guid id)
        {

            try
            {
                await _InventoryService.RemoveInventoryAsync(id);
                return Ok("");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating Inventory for user");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class InventoryV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Inventory API v2 example");
    }
}