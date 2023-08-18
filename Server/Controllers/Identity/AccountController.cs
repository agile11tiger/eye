using EyE.Server.Services;
using EyE.Shared.Enums;
using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using EyE.Shared.Models.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Azure;
using EyE.Shared.Models.Common;

namespace EyE.Server.Controllers.Identity
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        public AccountController(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            TokenService tokenService,
            EmailService emailService,
            IStringLocalizer<AccountController> localizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.emailService = emailService;
            this.localizer = localizer;
        }

        private readonly UserManager<UserModel> userManager;
        private readonly SignInManager<UserModel> signInManager;
        private readonly TokenService tokenService;
        private readonly EmailService emailService;
        private readonly IStringLocalizer<AccountController> localizer;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = new RegisterResponseModel();

            if ((await userManager.FindByEmailAsync(model.Email)) != null)
            {
                response.Messages = new List<string> { localizer["UserExists", model.Email] }; 
                return BadRequest(response);
            }

            var user = new UserModel { Email = model.Email, UserName = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && (await userManager.AddToRoleAsync(user, Roles.User.ToString())).Succeeded)
            {
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = confirmationToken },
                    protocol: HttpContext.Request.Scheme);
                await emailService.SendEmailAsync(
                    model.Email, 
                    localizer["RegisterConfirmHeader"], 
                    localizer["RegisterConfirmMessage",
                    callbackUrl]);
                response.Messages = new List<string> { localizer["RegistrationCompletionMessage"] };
                return Ok(response);
            }

            response.Messages = result.GetMessages();
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            var response = new ResponseModel();

            if (user == null || token == null)
            {
                response.Messages = new List<string> { localizer["UserNotExists", ""] };
                return BadRequest(response);
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View();

            response.Messages = result.GetMessages();
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    true);

            var user = await userManager.FindByNameAsync(model.Email);
            var response = new LoginResponseModel() { UserId = user.Id };

            if (result.Succeeded)
            {
                var signingCredentials = tokenService.GetSigningCredentials();
                var claims = await tokenService.GetClaims(user);
                var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                user.RefreshToken = tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await userManager.UpdateAsync(user);
                response.Token = token;
                response.RefreshToken = user.RefreshToken;
                return Ok(response);
            }

            response.Messages = new List<string> { localizer["WrongLoginOrPassword"] };
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var response = new ResponseModel();

            if (user == null)
            {
                response.Messages = new List<string> { localizer["UserNotExists", ""] };
                return BadRequest(response);
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                response.Messages = new List<string> { localizer["EmailNotConfirmed"] };
                return BadRequest(response);
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            await emailService.SendEmailAsync(
                model.Email, 
                localizer["PasswordResetMessageHeader"], 
                localizer["PasswordResetMessage", resetToken]);
            
            response.Messages = new List<string> { localizer["CodeSentToEmail"] };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var response = new ResponseModel();

            if (user == null)
            {
                response.Messages = new List<string> { localizer["UserNotExists", model.Email] };
                return BadRequest(response);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
                return await Login(new LoginViewModel() { Email = model.Email, Password = model.Password });

            response.Messages = result.GetMessages();
            return BadRequest(response);
        }


        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(refreshToken.Token);
            var user = await userManager.FindByEmailAsync(principal.Identity.Name);
            var response = new RefreshTokenViewModel();

            if (user == null || user.RefreshToken != refreshToken.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest(response.Messages = new List<string> { "Can`t refresh JSON Web Token" });

            var signingCredentials = tokenService.GetSigningCredentials();
            var claims = await tokenService.GetClaims(user);
            var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = tokenService.GenerateRefreshToken();

            await userManager.UpdateAsync(user);
            response.Token = token;
            response.RefreshToken = user.RefreshToken;

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
