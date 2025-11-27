using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventorySrv.Dtos;
using InventorySrv.Models;
using InventorySrv.Repos;
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
        /// Get all users data, Not in the requirement, just for test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventorys()
        {
            var Inventorys = await this._InventoryService.GetInventorysAsync();
            return Ok(Inventorys);
        }

        [HttpGet("data")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InventoryReadDto>>> GetUserInventorys(
            [FromHeader(Name = "x-userId")] string userId,
            [FromQuery] int? start = null,
            [FromQuery] int? end = null)
        {
            try
            {
                var InventoryDtos = await _InventoryService.GetInventorysAsync(userId, start, end);

                if (!InventoryDtos.Any())
                    return NotFound($"User {userId} not found");

                return Ok(new InventoryReadResponse { Data = InventoryDtos });
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

        [HttpPost("data")]
        [Authorize]
        public async Task<ActionResult<InventoryReadDto>> CreateInventoryForUser(
                                [FromHeader(Name = "x-userId")] string userId,
                                [FromBody] InventoryCreateDto InventoryCreateDto)
        {
            /*
             * Suppose we just record one place Inventory.
             1. if different users create opposite data, how to handle?
                [since we retrieve the data by user, I will record all data]
             2. can one day record multiple records?
                [one day can record multiple records, timestamp is different]
             */
            try
            {
                var createdInventory = await _InventoryService.CreateInventoryForUserAsync(userId, InventoryCreateDto);
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