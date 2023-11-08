using EyEServer.Services;
using EyEServer.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    SignInManager<UserModel> _signInManager)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var response = new RegisterResponseModel();

        if ((await _userManager.FindByEmailAsync(model.Email)) != null)
        {
            response.Messages = new List<string> { IdentityResource.UserExists.Format(model.Email) };
            return BadRequest(response);
        }

        if ((await _userManager.FindByNameAsync(model.Nickname)) != null)
        {
            response.Messages = new List<string> { IdentityResource.UserExists.Format(model.Nickname) };
            return BadRequest(response);
        }

        var user = new UserModel { Email = model.Email, UserName = model.Nickname };
        var createResult = await _userManager.CreateAsync(user, model.Password);

        if (createResult.Succeeded && (await _userManager.AddToRoleAsync(user, Roles.User.ToString())).Succeeded)
        {
            await SendEmailAsync(user, response);
            return Ok(response);
        }

        response.Messages = createResult.GetMessages();
        return BadRequest(response);
    }


    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailViewModel confirmEmailModel)
    {
        var response = new ResponseModel();
        var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);

        if (user == null || confirmEmailModel.Token == null)
        {
            response.Messages = new List<string> { IdentityResource.UserNotExists.Format(" ") };
            return BadRequest(response);
        }

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);

        if (result.Succeeded)
            return View();

        response.Messages = result.GetMessages();
        return BadRequest(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var response = new LoginResponseModel();

        if (user == default)
        {
            response.Messages = new List<string> { IdentityResource.UserNotExists.Format(" ") };
            return BadRequest(response);
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            await SendEmailAsync(user, response);
            return BadRequest(response);
        }

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

        if (result.Succeeded)
        {
            var claims = await _tokenService.GetClaims(user);
            var signingCredentials = _tokenService.GetSigningCredentials();
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(90);
            await _userManager.UpdateAsync(user);
            response.Nickname = user.UserName;
            response.Token = token;
            response.RefreshToken = user.RefreshToken;
            return Ok(response);
        }

        response.Messages = new List<string> { IdentityResource.WrongLoginOrPassword };
        return BadRequest(response);
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var response = new ResponseModel();
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            response.Messages = new List<string> { IdentityResource.UserNotExists.Format(" ") };
            return BadRequest(response);
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            response.Messages = new List<string> { IdentityResource.EmailNotConfirmed };
            return BadRequest(response);
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendEmailAsync(
            model.Email,
            IdentityResource.PasswordResetMessageHeader,
            IdentityResource.PasswordResetMessage.Format(resetToken));

        response.Messages = new List<string> { IdentityResource.CodeSentToEmail };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var response = new ResponseModel();
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            response.Messages = new List<string> { IdentityResource.UserNotExists.Format(model.Email) };
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

    private async Task SendEmailAsync(UserModel userModel, ResponseModel response)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
        var callbackUrl = Url.Action(
            nameof(ConfirmEmail),
            "Account",
            new ConfirmEmailViewModel { UserId = userModel.Id, Token = confirmationToken },
            protocol: HttpContext.Request.Scheme);
        await _emailService.SendEmailAsync(
            userModel.Email,
            IdentityResource.RegisterConfirmHeader,
            IdentityResource.RegisterConfirmMessage.Format(callbackUrl));
        response.Messages = new List<string> { IdentityResource.RegistrationCompletionMessage };
    }
}
