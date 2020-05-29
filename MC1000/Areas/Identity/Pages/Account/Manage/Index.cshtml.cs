using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MC1000.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MC1000.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Image { get; set; }
            public IFormFile ImageUpload { get; set; }
        }


        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var street = user.StreetName;
            var houseNr = user.HouseNumber;
            var zip = user.ZipCode;
            var city = user.City;
            var country = user.Country;
            var image = user.Image;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Street = street,
                HouseNumber = houseNr,
                ZipCode = zip,
                City = city,
                Country = country,
                Image = image

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.StreetName = Input.Street;
            user.HouseNumber = Input.HouseNumber;
            user.ZipCode = Input.ZipCode;
            user.City = Input.City;
            user.Country = Input.Country;

            //Load the image
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

            _ = await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
