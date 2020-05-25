using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class User : IdentityUser
    {
        // Veel van de properties uit de normalisatie tabel staan al in de IdentityUser class
        public override string Id { get; set; }
        public override string Email { get; set; }
        public override string UserName { get; set; }
        public string Image { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsEnabled { get; set; }
    }
}
