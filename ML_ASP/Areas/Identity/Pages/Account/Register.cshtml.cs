// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using ML_ASP.Models.Models;
using ML_ASP.Utility;

namespace ML_ASP.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Account_Model> _signInManager;
        private readonly UserManager<Account_Model> _userManager;
        private readonly IUserStore<Account_Model> _userStore;
        private readonly IUserEmailStore<Account_Model> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unit;

        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public bool IsAdminRegistration { get; set; }

        public RegisterModel(
            UserManager<Account_Model> userManager,
            IUserStore<Account_Model> userStore,
            SignInManager<Account_Model> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment environment,
            IUnitOfWork unit)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _environment = environment;
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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [BindProperty]
            [Required]
            [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be at least 16 characters long.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W)(?=.*[a-z]).*$")]
            public string Password { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public DateTime Birthdate{ get; set; }

            [Required]
            [DisplayName("Full Name")]
            public string FullName { get; set; }

            public string? PhoneNumber { get; set; }

            public string? Role { get; set; }

            public IFormFile Medical { get; set; }

            public IFormFile Enrollment { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            IsAdminRegistration = HttpContext.Request.Query["admin"] == "true";

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Unregistered)).GetAwaiter().GetResult();
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.PhoneNumber = Input.PhoneNumber;
                user.FullName = Input.FullName;
                user.Birthdte = Input.Birthdate;

                //REQUIREMENT REGISTRATION
                
                if (Input.Enrollment != null && Input.Enrollment.Length > 0 && Input.Medical != null && Input.Medical.Length > 0)
                {
                    string projectPath = _environment.WebRootPath;
                    string uploadFolderName = "RequirementFiles";
                    var uploads = Path.Combine(projectPath, uploadFolderName);

                    string fileName = Guid.NewGuid().ToString();
                    string secondFileName = Guid.NewGuid().ToString();

                    var extension = Path.GetExtension(Input.Enrollment.FileName);
                    var secondExtension = Path.GetExtension(Input.Medical.FileName);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    string newEnrollmentFileName = fileName + extension;
                    string newMedicalFileName = secondFileName + secondExtension;

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        Input.Enrollment.CopyTo(fileStream);
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, secondFileName + secondExtension), FileMode.Create))
                    {
                        Input.Medical.CopyTo(fileStream);
                    }

                    user.Enrollment = newEnrollmentFileName;
                    user.Medical = newMedicalFileName;
                }
                //REQUIREMENT REGISTRATION: ENDS-----------
                {
                    RequirementForm_Model formModel = new RequirementForm_Model();
                    RequirementForm_Model formModel2 = new RequirementForm_Model();
                    RequirementForm_Model formModel3 = new RequirementForm_Model();
                    RequirementForm_Model formModel4 = new RequirementForm_Model();
                    RequirementForm_Model formModel5 = new RequirementForm_Model();
                    //REQUIREMENTFORM START--------------------
                    formModel.IsSubmitted = false;
                    formModel.UserId = user.Id;
                    formModel.FileName = "Null";
                    formModel.FormNumber = 1;

                    formModel2.IsSubmitted = false;
                    formModel2.UserId = user.Id;
                    formModel2.FileName = "Null";
                    formModel2.FormNumber = 2;

                    formModel3.IsSubmitted = false;
                    formModel3.UserId = user.Id;
                    formModel3.FileName = "Null";
                    formModel3.FormNumber = 3;


                    formModel4.IsSubmitted = false;
                    formModel4.UserId = user.Id;
                    formModel4.FileName = "Null";
                    formModel4.FormNumber = 4;

                    formModel5.IsSubmitted = false;
                    formModel5.UserId = user.Id;
                    formModel5.FileName = "Null";
                    formModel5.FormNumber = 5;

                    _unit.RequirementForm.Add(formModel);
                    _unit.RequirementForm.Add(formModel2);
                    _unit.RequirementForm.Add(formModel3);
                    _unit.RequirementForm.Add(formModel4);
                    _unit.RequirementForm.Add(formModel5);

                    _unit.Save();
                }
                //REQUIREMENT FORM ENDS-------------------

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (Input.Role == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Admin);
                }
                else if(!IsAdminRegistration)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Unregistered);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_User);
                }


                if (result.Succeeded)
                {
                    bool isAdmin = await _userManager.IsInRoleAsync(user, SD.Role_Admin);

                    if (user.RegistrationPermission == "Pending" && !isAdmin) //pending permission
                    {
                        returnUrl ??= Url.Content("~/RequirementFile/Index");
                    }
                    else if (user.RegistrationPermission == "Approved") //accepted permission
                    {
                        returnUrl ??= Url.Content("~/Dashboard/Dashboard");
                    }
                    else if (user.RegistrationPermission == "Declined") //denied permission, re register, failed registration account will be deleted in 2 days.
                    {
                        returnUrl ??= Url.Content("~/RequirementFile/PermissionDenied");
                    }
                    if (isAdmin)
                    {
                        returnUrl ??= Url.Content("~/Admin/Admin");
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Account_Model CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Account_Model>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Account_Model)}'. " +
                    $"Ensure that '{nameof(Account_Model)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Account_Model> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Account_Model>)_userStore;
        }
    }
}
