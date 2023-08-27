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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;
        public AuthController(UserManager<IdentityUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Registers a user to the database.
        /// </summary>
        /// <param name="userRegistrerRequestDto">The details of the user to be registered.</param>
        /// <returns>A new Jwt token.</returns>
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegistrerRequestDto userRegistrerRequestDto)
        {
            _logger.LogInformation("Registering new user");

            //validate income request
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid request");

                return BadRequest();
            }

            //check if email already exists
            var userExists = await _userManager.FindByEmailAsync(userRegistrerRequestDto.Email);
            if (userExists != null)
            {
                _logger.LogInformation("Email already exists");

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
                _logger.LogInformation("Server error");

                return BadRequest("Server error");
            }


            //add user to a role
            _logger.LogInformation("Adding user to role");

            var roleAddition = await _userManager.AddToRoleAsync(newUser, "User");

            //generate the token
            _logger.LogInformation("Generating token");

            var token = await _jwtService.GenerateJwtToken(newUser);
            _logger.LogInformation("Generation successfull");

            return Ok(token);

        }

        /// <summary>
        /// The user logs in.
        /// </summary>
        /// <param name="userLoginRequestDto">The details of the user trying to log in.</param>
        /// <returns>A new Jwt token.</returns>
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto userLoginRequestDto)
        {
            _logger.LogInformation("User log in");

            //validate
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid information");

                return BadRequest("Invalid information");
            }

            //check if user exists
            var existingUser = await _userManager.FindByEmailAsync(userLoginRequestDto.Email);
            if (existingUser == null)
            {
                _logger.LogInformation("User doesn't exist");

                return BadRequest("User doesn't exist");
            }

            //check if password is correct
            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLoginRequestDto.Password);
            if (!isCorrect)
            {
                _logger.LogInformation("Wrong password");

                return BadRequest("Wrong password");
            }
            //generate token
            var jwtToken =await _jwtService.GenerateJwtToken(existingUser);
            _logger.LogInformation("Jwt generated successfully");

            return Ok(jwtToken);
        }


        /// <summary>
        /// Refreshes the Jwt token of the user.
        /// </summary>
        /// <param name="tokenRequest">The old token.</param>
        /// <returns>The new Jwt and refresh tokens.</returns>
        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequest)
        {
            _logger.LogInformation("Refreshing token");

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid parameters");
                return BadRequest("Invalid parameters");
            }
            var result = await _jwtService.VerifyAndGenerateToken(tokenRequest);
            if (result == null)
            {
                _logger.LogInformation("Invalid tokens");

                return BadRequest("Invalid Tokens");
            }
            _logger.LogInformation("Refresh was successful");

            return Ok(result);
        }

    }
}
