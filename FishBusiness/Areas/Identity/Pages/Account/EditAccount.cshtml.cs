using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace FishBusiness.Areas.Identity.Pages.Account
{
   
    public class EditAccountModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public EditAccountModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            Input = new InputModel();
            var userr = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(userr);
            if (roles.Contains("admin"))
            {
                var p = _context.People.Find(1);
                Input.Name = p.Name;
            }
            else
            {
                var p = _context.People.Find(2);
                Input.Name = p.Name;
            }
            Input.Email = userr.Email;
            Input.OldPassword = "";
            //ReturnUrl = returnUrl;
           // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var userr = await _userManager.GetUserAsync(User);
            //returnUrl = returnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                userr.Email = Input.Email;
                userr.UserName = Input.Email;
                var roles = await _userManager.GetRolesAsync(userr);
                if (roles.Contains("admin"))
                {
                    var p = _context.People.Find(1);
                    p.Name = Input.Name;
                }
                else
                {
                    var p = _context.People.Find(2);
                    p.Name = Input.Name;
                }
                userr.Email = Input.Email;
               var changePasswordResult = await _userManager.ChangePasswordAsync(userr, Input.OldPassword, Input.Password);
                if (changePasswordResult.Succeeded)
                {
                    await _userManager.UpdateAsync(userr);
                    await _context.SaveChangesAsync();
                    await _signInManager.SignOutAsync();
                    return  RedirectToPage("./Login");
                }

                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        
        }
    }
}
