using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BurgerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;

        public SetupController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<SetupController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves all the existing roles.
        /// </summary>
        /// <returns>A list of roles.</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            _logger.LogInformation("Getting all roles");

            var roles = await _roleManager.Roles.ToListAsync();

            _logger.LogInformation("Roles retrieved successfully");

            return Ok(roles);
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        /// <returns>A response message.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateRole(string name)
        {
            _logger.LogInformation("Creating new role");

            //check if role already exists
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (roleExist)
            {
                _logger.LogInformation("Role already exists");

                return BadRequest("Role already exists");
            }

            //check if role was added successfully
            var roleResult = await _roleManager.CreateAsync(new IdentityRole { Name = name, ConcurrencyStamp = Guid.NewGuid().ToString() });

            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Role wasnt added successfully");

                return BadRequest("Role was not added");
            }
            _logger.LogInformation("Role was added successfully");

            return Ok("Added successfully");
        }

        /// <summary>
        /// Retrieves all the users.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            _logger.LogInformation("Getting all users");

            var users = await _userManager.Users.ToListAsync();

            _logger.LogInformation("Users retrieved successfully");

            return Ok(users);
        }

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        /// <param name="email">The email of the user and the role to be added.</param>
        /// <param name="roleName">A response message.</param>
        /// <returns></returns>
        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string email, string roleName)
        {
            _logger.LogInformation("Adding user to role");

            //check if user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation("User doenst exist");

                return BadRequest("User doens't exist");
            }

            //check if role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation("Role doenst exist");

                return BadRequest("Role doesn't exist");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            //check if user assigned roled successfully
            if (!result.Succeeded)
            {
                _logger.LogInformation("Something went wrong");

                return BadRequest("Something went wrong");
            }
            _logger.LogInformation("Added user to role successfully");

            return Ok("User added to role successfully");
        }

        /// <summary>
        /// Gets the roles of a spesific user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>A list of roles.</returns>
        [HttpGet("GetUserRoles")]
        public async Task<ActionResult> GetUserRoles(string email) 
        {
            _logger.LogInformation("Getting the roles of a user given the email");

            //check if email is valid
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation("User doenst exist");

                return BadRequest("User doens't exist");
            }

            //return roles
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Roles retrieved successfully");

            return Ok(roles);
        }

        /// <summary>
        /// Removes a user from a spesific role.
        /// </summary>
        /// <param name="email">The email of the user and the role to be removed.</param>
        /// <param name="roleName">A response message.</param>
        /// <returns></returns>
        [HttpPost("RemoveUserFromRole")]
        public async Task<ActionResult> RemoveUserFromRole(string email, string roleName)
        {
            _logger.LogInformation("Removing user from role");

            //check if email is valid
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation("User doens't exist");

                return BadRequest("User doens't exist");
            }

            //check if role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation("Role doens't exist");

                return BadRequest("Role doesn't exist");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogInformation("Something went wrong");

                return BadRequest("Something went wrong");
            }
                _logger.LogInformation("User removed from role successfully");

            return Ok("User removed from role successfully");
        }
    }
}
