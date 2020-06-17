using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MC1000.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MC1000.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Het wachtwoord moet teminste {2} en maximaal {1} characters lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigde wachtwoord komt niet overeen.")]
            public string ConfirmPassword { get; set; }

            //[Display(Name = "Stad")]
            //public string City { get; set; }
            [Phone]
            [Display(Name = "Telefoonnummer")]
            public string PhoneNumber { get; set; }
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Image { get; set; }
            public IFormFile ImageUpload { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Email, Email = Input.Email};
                user.PhoneNumber = Input.PhoneNumber;
                user.StreetName = Input.Street;
                user.HouseNumber = Input.HouseNumber;
                user.ZipCode = Input.ZipCode;
                user.City = Input.City;
                user.Country = Input.Country;
                var imageUpload = Input.ImageUpload;
                if (imageUpload != null)
                {
                    string g = Guid.NewGuid().ToString();

                    var fileExtension = Path.GetExtension(imageUpload.FileName);
                    var filePath = Url.Content("wwwroot/uploads/images/avatars/" + g + "." + user.Id + fileExtension);

                    var url = g + "." + user.Id + fileExtension;
                    user.Image = url;

                    if (imageUpload.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageUpload.CopyToAsync(stream);
                        }
                    }
                    user.Image = url;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Gebruiker heeft een nieuw account gemaakt met wachtwoord.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Bevestig alstublieft uw account door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier te klikken</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
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
    }
}
