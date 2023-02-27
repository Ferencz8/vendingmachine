using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.API.Models;
using VendingMachine.DAL;
using VendingMachine.DAL.Entities;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] User model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            }

            bool doesRoleExist = await _roleManager.RoleExistsAsync(model.Role);
            if (!doesRoleExist)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Specified user role does not exist!" });
            }

            AppUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Deposit = model.Deposit
            };

            bool result = false;
            //https://stackoverflow.com/questions/36636272/transactions-with-asp-net-identity-usermanager
            using(var identityDbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var resultCreate = await _userManager.CreateAsync(user, model.Password);
                    if (resultCreate.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                        identityDbTransaction.Commit();
                        result = true;
                    }
                }
                catch(Exception ex)
                {
                    //TODO:: log
                }
                finally
                {
                    identityDbTransaction.Rollback();
                }
            }
            

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(username);
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "User not found!" });
            
            var roles = await _userManager.GetRolesAsync(user);
            
            return Ok(new User{ Deposit = user.Deposit, Role = roles.FirstOrDefault(), Username = username });
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] User model)
        {
            //TODO:: security needs to have Admin role
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "User not found!" });


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User update failed! Please check user details and try again." });

            return Ok(new { Status = "Success", Message = "User updated successfully!" });
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Remove([FromQuery] string username)
        {

            //TODO:: security needs to have Admin role
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "User not found!" });

            
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User removal failed! Please check user details and try again." });

            return Ok(new { Status = "Success", Message = "User removed successfully!" });
        }
    }
}
