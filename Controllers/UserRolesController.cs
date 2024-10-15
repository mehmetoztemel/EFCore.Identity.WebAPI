using EFCore.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        public UserRolesController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(Guid userId, string roleName, CancellationToken cancellationToken)
        {
            AppUser? appUser = await _userManager.FindByIdAsync(userId.ToString());
            if (appUser is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }

            IdentityResult result = await _userManager.AddToRoleAsync(appUser, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Errors.SelectMany(x => x.Description) });
            }
            return Ok(new { Message = "Role added to user" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoleFromUser(Guid userId, string roleName, CancellationToken cancellationToken)
        {
            AppUser? appUser = await _userManager.FindByIdAsync(userId.ToString());
            if (appUser is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }

            IdentityResult result = await _userManager.RemoveFromRoleAsync(appUser, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Errors.SelectMany(x => x.Description) });
            }
            return Ok(new { Message = "Role removed from on  user" });
        }




    }
}
