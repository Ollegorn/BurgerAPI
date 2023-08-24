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
        public BurgerController(IBurgerGetterService burgerGetterService)
        {
            _getterService = burgerGetterService;
            
        }

        [HttpGet("AllBurgers")]
        public async Task<ActionResult<List<BurgerResponseDto>>> GetAllBurgers()
        {
            //logg
            var burgers = await _getterService.GetAllBurgers();
            //logg
            return Ok(burgers);
        }

        [HttpGet("ByIds")]
        public async Task<ActionResult<List<BurgerResponseDto>>> GetBurgersByIds([FromQuery] List<int> ids)
        {
            //logg
            var burgers = await _getterService.GetBurgersByIds(ids);
            //logg
            return Ok(burgers);
        }

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

    }
}
