using Microsoft.AspNetCore.Http;
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
        public BurgerController(IBurgerGetterService burgerGetterService, IBurgerAdderService adderService)
        {
            _getterService = burgerGetterService;
            _adderService = adderService;
        }
        /// <summary>
        /// Retrieves all Burgers.
        /// </summary>
        /// <returns>A list of Burgers.</returns>
        [HttpGet("AllBurgers")]
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

        [HttpPost]
        public async Task<ActionResult<BurgerResponseDto>> AddBurger(BurgerAddRequestDto burgerAddRequestDto)
        {
            var addedBurger = await _adderService.AddBurger(burgerAddRequestDto);

            return CreatedAtAction(nameof(GetBurgerById), new { id = addedBurger.Id }, addedBurger);
        }
    }
}
