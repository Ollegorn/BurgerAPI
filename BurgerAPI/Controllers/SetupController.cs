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
        private readonly BurgerDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SetupController(BurgerDbContext pizzaDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = pizzaDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(string name)
        {
            //check if role already exists
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (roleExist)
            {
                return BadRequest("Role already exists");
            }

            //check if role was added successfully
            var roleResult = await _roleManager.CreateAsync(new IdentityRole { Name = name, ConcurrencyStamp = Guid.NewGuid().ToString() });

            if (!roleResult.Succeeded)
            {
                return BadRequest("Role was not added");
            }

            return Ok("Added successfully");
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string email, string roleName)
        {
            //check if user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User doens't exist");
            }

            //check if role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Role doesn't exist");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            //check if user assigned roled successfully
            if (!result.Succeeded)
            {
                return BadRequest("Something went wrong");
            }
            return Ok("User added to role successfully");
        }

        [HttpGet("GetUserRoles")]
        public async Task<ActionResult> GetUserRoles(string email) 
        {
            //check if email is valid
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User doens't exist");
            }

            //return roles
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<ActionResult> RemoveUserFromRole(string email, string roleName)
        {
            //check if email is valid
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User doens't exist");
            }

            //check if role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Role doesn't exist");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest("Something went wrong");
            }
            return Ok("User removed from role successfully");
        }
    }
}
