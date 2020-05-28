using MC1000.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Create Roles
            if(roleManager.FindByNameAsync("Redactie").Result == null)
            {
                IdentityRole redactieRole = new IdentityRole("Redactie");
                roleManager.CreateAsync(redactieRole).Wait();
            }

            if (roleManager.FindByNameAsync("Customer").Result == null)
            {
                IdentityRole custRole = new IdentityRole("Customer");
                roleManager.CreateAsync(custRole).Wait();
            }

            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityRole adminRole = new IdentityRole { Name = "Admin" };
                roleManager.CreateAsync(adminRole).Wait();
            }

            if (roleManager.FindByNameAsync("Blocked").Result == null)
            {
                IdentityRole blockedRole = new IdentityRole { Name = "Blocked" };
                roleManager.CreateAsync(blockedRole).Wait();
            }


            //Create standard user
            if (userManager.FindByEmailAsync("customer@MC1000.com").Result == null)
            {
                User user = new User
                {
                    UserName = "customer@MC1000.com",
                    Email = "customer@MC1000.com",
                };

                IdentityResult result = userManager.CreateAsync(user, "Password123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Customer").Wait();
                }
            }

            //Create standard Moderator user
            if (userManager.FindByEmailAsync("redactie@MC1000.com").Result == null)
            {
                User user = new User
                {
                    UserName = "redactie@MC1000.com",
                    Email = "redactie@MC1000.com",
                };

                IdentityResult result = userManager.CreateAsync(user, "Password123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Redactie").Wait();
                }
            }

            //Create standard Admin user
            if (userManager.FindByEmailAsync("admin@MC1000.com").Result == null)
            {
                User user = new User
                {
                    UserName = "admin@MC1000.com",
                    Email = "admin@MC1000.com",
                };

                IdentityResult result = userManager.CreateAsync(user, "Password123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

        }
    }
}
