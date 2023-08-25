using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.AuthorizationDto;
using ServiceContracts.Interfaces;

namespace BurgerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(UserManager<IdentityUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;

        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegistrerRequestDto userRegistrerRequestDto)
        {
            //validate income request
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //check if email already exists
            var userExists = await _userManager.FindByEmailAsync(userRegistrerRequestDto.Email);
            if (userExists != null)
            {
                return BadRequest("Email already exists");
            }

            //create new user
            var newUser = new IdentityUser()
            {
                Email = userRegistrerRequestDto?.Email,
                UserName = userRegistrerRequestDto?.Email
            };
            var isCreated = await _userManager.CreateAsync(newUser, userRegistrerRequestDto.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest("Server error");
            }

            //add user to a role
            var roleAddition = await _userManager.AddToRoleAsync(newUser, "User");

            //generate the token
            var token = _jwtService.GenerateJwtToken(newUser);

            return Ok(token);

        }

    }
}
