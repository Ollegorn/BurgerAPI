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
        private readonly IJwtService _jwtService;
        public BurgerController(IBurgerGetterService burgerGetterService, IBurgerAdderService adderService, IBurgerUpdaterService updaterService, IBurgerDeleterService deleterService, IJwtService jwtService)
        {
            _getterService = burgerGetterService;
            _adderService = adderService;
            _updaterService = updaterService;
            _deleterService = deleterService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// Retrieves all Burgers.
        /// </summary>
        /// <returns>A list of Burgers.</returns>
        [HttpGet("AllBurgers")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult<List<BurgerResponseDto>>> GetAllBurgers()
        {
            //logg
            var burgers = await _getterService.GetAllBurgers();
            //logg
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
            //logg
            var burgers = await _getterService.GetBurgersByIds(ids);
            //logg
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
            //logg
            var burger = await _getterService.GetBurgerById(id);

            if (burger == null)
            {
                //logg
                return NotFound();
            }
            //logg
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
            //logg
            var addedBurger = await _adderService.AddBurger(BurgerAddRequestDto);
            //logg
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
            //logg
            var updateBurger = await _updaterService.UpdateBurger(burgerUpdateRequestDto);

            if (!updateBurger)
                return NotFound("Burger doesn't exist");

            return Ok("Updated Successfully");
        }

        /// <summary>
        /// Deletes a Burger base on the id provided.
        /// </summary>
        /// <param name="id">The id of the Burger to be deleted.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBurger(int id)
        {
            //logg
            var isDeleted = await _deleterService.DeleteBurgerById(id);
            
            if (!isDeleted)
                //loggg
                return NotFound("Not found");
            
            //logg
            return Ok("Deleted Successfully");
        
        }
    }
}
