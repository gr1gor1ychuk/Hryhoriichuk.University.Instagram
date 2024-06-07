using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Hryhoriichuk.University.Instagram.Web.Areas.Identity.Data;

namespace Hryhoriichuk.University.Instagram.Web
{
    public static class UserGenerator
    {
        public static async Task GenerateUsers(IServiceProvider serviceProvider, int userCount = 100)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            for (int i = 1; i <= userCount; i++)
            {
                var user = new ApplicationUser
                {
                    FullName = $"User {i}",
                    UserName = $"user{i}",
                    Email = $"user{i}@example.com",
                    ProfilePicturePath = "https://i.pinimg.com/736x/0d/64/98/0d64989794b1a4c9d89bff571d3d5842.jpg",
                    IsPrivate = false
                };

                var result = await userManager.CreateAsync(user, "Password123!");

                if (result.Succeeded)
                {
                    Console.WriteLine($"User {i} created successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to create user {i}: {string.Join(", ", result.Errors)}");
                }
            }
        }
    }
}
