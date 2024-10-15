using EFCore.Identity.WebAPI.Dtos;
using EFCore.Identity.WebAPI.Models;
using EFCore.Identity.WebAPI.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Identity.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {
            AppUser appUser = new AppUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await Task.CompletedTask;
            return Ok(new { Message = "Registration completed successfully" });
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description).ToList());
            }

            return Ok(new { Message = "Password changed successfully" });
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
        {

            AppUser? appUser = await _userManager.FindByEmailAsync(email);
            if (appUser is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

            string newPassword = PasswordGenerator.GenerateRandomPassword(8);
            IdentityResult result = await _userManager.ResetPasswordAsync(appUser, token, newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description).ToList());
            }
            return Ok(new { Message = $"New Password: {newPassword}" });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await _userManager.Users
                .FirstOrDefaultAsync(p => p.UserName == request.UserNameOrEmail ||
                p.Email == request.UserNameOrEmail, cancellationToken);

            if (appUser is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }
            bool passwordCheck = await _userManager.CheckPasswordAsync(appUser, request.Password);
            if (!passwordCheck)
            {
                return BadRequest(new { Message = "Password is wrong" });
            }

            return Ok(new { Token = "Token" });
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithSignInManager(LoginDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail ||
                u.Email == request.UserNameOrEmail, cancellationToken);
            if (appUser is null)
            {
                return BadRequest(new { Message = "User cannot find" });
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

            if (result.IsLockedOut)
            {
                //var lockoutEndLocal = DateTime.Now.AddMinutes(1); // Yerel saat diliminde ekleme
                //await _userManager.SetLockoutEndDateAsync(appUser, lockoutEndLocal);

                return StatusCode(500, "Your account has been locked for 5 minutes because you entered your password incorrectly 3 times.");
            }
            if (!result.Succeeded)
            {
                return StatusCode(500, "Password is wrong");
            }
            if (result.IsNotAllowed)
            {
                return StatusCode(500, "You must confirm your e-mail address.");
            }
            return Ok(new { Token = "Token" });
        }
    }


}