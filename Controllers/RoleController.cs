using EFCore.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Identity.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private RoleManager<AppRole> _roleManager;
        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole(string name, CancellationToken cancellationToken)
        {
            AppRole appRole = new AppRole()
            {
                Name = name
            };
            IdentityResult result = await _roleManager.CreateAsync(appRole);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Errors.SelectMany(x => x.Description) });
            }
            return Ok(new { Message = "Role Created" });
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            return Ok(roles);
        }
    }
}
