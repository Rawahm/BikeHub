using BikeHub.Areas.Identity.Pages.Account;
using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
/*
 ***
 *Author:Rawah Almesri
 *Date : Nov 2,2024
 *Description:
 *This Controller used to reset password for the admin
 *include forgot password and Reset password
 */

namespace BikeHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly BikeHubDBContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<AccountController> logger;
        public AccountController(BikeHubDBContext dbContext, UserManager<IdentityUser> userManager,ILogger<AccountController> logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);

                    // Pass the password reset link to the confirmed view
                    ViewBag.PasswordResetLink = passwordResetLink;
                    return View("ForgotPasswordConfirmation");
                }

                // handle the case for an unconfirmed email
                ViewBag.ErrorMessage = "Email is not confirmed. You have to confirm your email to be able to reset your password.";
                return View("ForgotPasswordConfirmation");
            }

            // If the model state is invalid, return the current model to show validation errors
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token,string email) {
        
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid) { 
            var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user,model.Token,model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
    }
    }

