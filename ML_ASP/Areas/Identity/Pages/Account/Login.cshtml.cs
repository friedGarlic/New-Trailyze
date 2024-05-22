// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ML_ASP.Utility;
using System.Security.Claims;
using ML_ASP.Models;
using ML_ASP.DataAccess.Repositories.IRepositories;

namespace ML_ASP.Areas.Identity.Pages.Account
{

    public class LoginModel : PageModel
    {
        private readonly SignInManager<Account_Model> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<Account_Model> _userManager;
        private readonly IUnitOfWork _unit;

        public LoginModel(SignInManager<Account_Model> signInManager,
            ILogger<LoginModel> logger,
            UserManager<Account_Model> userManager,
            IUnitOfWork unit)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _unit = unit;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                bool isAdmin = await _userManager.IsInRoleAsync(user, SD.Role_Admin);

                if (user.RegistrationPermission == 0 && !isAdmin) //pending permission
                {
                    returnUrl ??= Url.Content("~/RequirementFile/Index");
                }
                else if (user.RegistrationPermission == 1) //accepted permission
                {
                    returnUrl ??= Url.Content("~/Dashboard/Dashboard");
                }
                else if (user.RegistrationPermission == 2) //denied permission, re register, failed registration account will be deleted in 2 days.
                {
                    returnUrl ??= Url.Content("~/RequirementFile/PermissionDenied");
                }
                if (isAdmin)
                {
                    returnUrl ??= Url.Content("~/Admin/Admin");
                }
            }
            catch
            {

            }
            
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<Account_Model> GetUserFromClaimsAsync(ClaimsPrincipal claimsPrincipal)
        {
            // Get the user's ID claim from the ClaimsPrincipal
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                // Find the user by ID using UserManager
                var user = await _userManager.FindByIdAsync(userIdClaim.Value);

                // Optionally, you can also load additional user information if needed
                if (user != null)
                {
                    // Load additional claims if necessary
                    var additionalClaims = claimsPrincipal.Claims.Where(c => c.Type != ClaimTypes.NameIdentifier);
                    await _userManager.AddClaimsAsync(user, additionalClaims);
                }

                return user;
            }

            return null;
        }
    }
}
