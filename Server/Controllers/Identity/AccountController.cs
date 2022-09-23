using EyE.Server.Extensions;
using EyE.Server.Services;
using EyE.Shared.Enums;
using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace EyE.Server.Controllers.Identity
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            EmailService emailService,
            IStringLocalizer<AccountController> localizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.localizer = localizer; // TODO хз как используется
        }

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly EmailService emailService;
        private readonly IStringLocalizer<AccountController> localizer;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if ((await userManager.FindByEmailAsync(model.Email)) != null)
            {
                ModelState.AddModelError("", localizer["UserExists", model.Email]);
                return BadRequest(ModelState);
            }

            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && (await userManager.AddToRoleAsync(user, Roles.User.ToString())).Succeeded)
            {
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = confirmationToken },
                    protocol: HttpContext.Request.Scheme);
                await emailService.SendEmailAsync(model.Email, localizer["RegisterConfirmHeader"], localizer["RegisterConfirmMessage", callbackUrl]);
                return Ok(localizer["RegistrationCompletionMessage"].Value);
            }
            else
                ModelState.AddModelErrors(result);

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null || token == null)
            {
                ModelState.AddModelError("", localizer["UserNotExists", ""]);
                return BadRequest(ModelState);
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View();
            else
                ModelState.AddModelErrors(result);

            return BadRequest(ModelState);
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
                    false);

            if (result.Succeeded)
                return Ok();
            else
                ModelState.AddModelError("", localizer["WrongLoginOrPassword"]);

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", localizer["UserNotExists", model.Email]);
                return BadRequest(ModelState);
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", localizer["EmailNotConfirmed"]);
                return BadRequest(ModelState);
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            await emailService.SendEmailAsync(model.Email, localizer["PasswordResetMessageHeader"], localizer["PasswordResetMessage", resetToken]);

            return Ok(localizer["CodeSentToEmail"].Value);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", localizer["UserNotExists", model.Email]);
                return BadRequest(ModelState);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
                return await Login(new LoginViewModel() { Email = model.Email, Password = model.Password });
            else
                ModelState.AddModelErrors(result);

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
