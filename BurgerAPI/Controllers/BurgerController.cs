using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;

namespace BurgerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BurgerController : ControllerBase
    {
        private readonly IBurgerGetterService _getterService;
        private readonly IBurgerAdderService _adderService;
        private readonly IBurgerUpdaterService _updaterService;
        private readonly IBurgerDeleterService _deleterService;
        private readonly ILogger<BurgerController> _logger;
        public BurgerController(IBurgerGetterService burgerGetterService, IBurgerAdderService adderService, IBurgerUpdaterService updaterService, IBurgerDeleterService deleterService, ILogger<BurgerController> logger)
        {
            _getterService = burgerGetterService;
            _adderService = adderService;
            _updaterService = updaterService;
            _deleterService = deleterService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves all Burgers.
        /// </summary>
        /// <returns>A list of Burgers.</returns>
        [HttpGet("AllBurgers")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult<List<BurgerResponseDto>>> GetAllBurgers()
        {
            _logger.LogInformation("Retrieving all burger");

            var burgers = await _getterService.GetAllBurgers();

            _logger.LogInformation("All burgers retrieved successfully");

            return Ok(burgers);
        }

        /// <summary>
        /// Retrieves any existing Burger if the id is matching the given ids.
        /// </summary>
        /// <param name="ids">A list of ids.</param>
        /// <returns>A list of Burgers.</returns>
        [HttpGet("BurgersByIds")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<BurgerResponseDto>>> GetBurgersByIds([FromQuery] List<int> ids) 
        {
            _logger.LogInformation("Retrieving all burger based on id");

            var burgers = await _getterService.GetBurgersByIds(ids);

            _logger.LogInformation("Burgers retrieved successfully");

            return Ok(burgers);
        }

        /// <summary>
        /// Retrieves an existing Burger by id.
        /// </summary>
        /// <param name="id">An id of a burger.</param>
        /// <returns>A burger.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BurgerResponseDto>> GetBurgerById(int id)
        {
            _logger.LogInformation($"Retrieving burger based on id: {id}");
            var burger = await _getterService.GetBurgerById(id);

            if (burger == null)
            {
                _logger.LogInformation("Burger not found");
                return NotFound();
            }
            _logger.LogInformation("Burger was found");
            return Ok(burger);
        }

        /// <summary>
        /// Adds a new Burger.
        /// </summary>
        /// <param name="BurgerAddRequestDto">The Details of the Burger.</param>
        /// <returns>The added Burger.</returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<BurgerResponseDto>> AddBurger(BurgerAddRequestDto BurgerAddRequestDto)
        {
            _logger.LogInformation("Adding a new burger");

            var addedBurger = await _adderService.AddBurger(BurgerAddRequestDto);

            _logger.LogInformation("Burger added successfully");

            return CreatedAtAction(nameof(GetBurgerById), new { id = addedBurger.Id }, addedBurger);
        }

        /// <summary>
        /// Updates an existing Burger.
        /// </summary>
        /// <param name="burgerUpdateRequestDto">The id of the Burger to be updated and the new details.</param>
        /// <returns>The updated Burger.</returns>
        [HttpPut("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBurger(BurgerUpdateRequestDto burgerUpdateRequestDto)
        {
            _logger.LogInformation("Updating burger!");
            var updateBurger = await _updaterService.UpdateBurger(burgerUpdateRequestDto);

            if (!updateBurger)
            {
                _logger.LogInformation("Burger not found");

                return NotFound("Burger doesn't exist");
            }
            _logger.LogInformation("Found Burger");
            return Ok("Updated Successfully");
        }

        /// <summary>
        /// Deletes a Burger based on the id provided.
        /// </summary>
        /// <param name="id">The id of the Burger to be deleted.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBurger(int id)
        {
            _logger.LogInformation("Deleting burger with given id");
            var isDeleted = await _deleterService.DeleteBurgerById(id);
            
            if (!isDeleted)
            {
                _logger.LogInformation("Burger not found");
                return NotFound("Not found");
            }
            
            _logger.LogInformation("Burger deleted successfully");

            return Ok("Deleted Successfully");
        
        }
    }
}
