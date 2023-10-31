using EyEServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
namespace EyEServer.Controllers.Identity;

[Route("api/[controller]/[action]")]
public class AccountController(
    TokenService _tokenService,
    EmailService _emailService,
    UserManager<UserModel> _userManager,
    SignInManager<UserModel> _signInManager,
    IStringLocalizer<AccountController> _localizer)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var response = new RegisterResponseModel();

        if ((await _userManager.FindByEmailAsync(model.Email)) != null)
        {
            response.Messages = new List<string> { _localizer["UserExists", model.Email] };
            return BadRequest(response);
        }

        var user = new UserModel { Email = model.Email, UserName = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded && (await _userManager.AddToRoleAsync(user, Roles.User.ToString())).Succeeded)
        {
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token = confirmationToken },
                protocol: HttpContext.Request.Scheme);
            await _emailService.SendEmailAsync(
                model.Email,
                _localizer["RegisterConfirmHeader"],
                _localizer["RegisterConfirmMessage",
                callbackUrl]);
            response.Messages = new List<string> { _localizer["RegistrationCompletionMessage"] };
            return Ok(response);
        }

        response.Messages = result.GetMessages();
        return BadRequest(response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var response = new ResponseModel();

        if (user == null || token == null)
        {
            response.Messages = new List<string> { _localizer["UserNotExists", ""] };
            return BadRequest(response);
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

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
        var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                true);

        var user = await _userManager.FindByNameAsync(model.Email);
        var response = new LoginResponseModel() { UserId = user.Id };

        if (result.Succeeded)
        {
            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            response.Token = token;
            response.RefreshToken = user.RefreshToken;
            return Ok(response);
        }

        response.Messages = new List<string> { _localizer["WrongLoginOrPassword"] };
        return BadRequest(response);
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var response = new ResponseModel();

        if (user == null)
        {
            response.Messages = new List<string> { _localizer["UserNotExists", ""] };
            return BadRequest(response);
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            response.Messages = new List<string> { _localizer["EmailNotConfirmed"] };
            return BadRequest(response);
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendEmailAsync(
            model.Email,
            _localizer["PasswordResetMessageHeader"],
            _localizer["PasswordResetMessage", resetToken]);

        response.Messages = new List<string> { _localizer["CodeSentToEmail"] };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var response = new ResponseModel();

        if (user == null)
        {
            response.Messages = new List<string> { _localizer["UserNotExists", model.Email] };
            return BadRequest(response);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

        if (result.Succeeded)
            return await Login(new LoginViewModel() { Email = model.Email, Password = model.Password });

        response.Messages = result.GetMessages();
        return BadRequest(response);
    }


    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken.Token);
        var user = await _userManager.FindByEmailAsync(principal.Identity.Name);
        var response = new RefreshTokenViewModel();

        if (user == null || user.RefreshToken != refreshToken.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest(response.Messages = new List<string> { "Can`t refresh JSON Web Token" });

        var signingCredentials = _tokenService.GetSigningCredentials();
        var claims = await _tokenService.GetClaims(user);
        var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        user.RefreshToken = _tokenService.GenerateRefreshToken();

        await _userManager.UpdateAsync(user);
        response.Token = token;
        response.RefreshToken = user.RefreshToken;

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}
