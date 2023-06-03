using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Models.Entities;

namespace YATM.Data.Seeds
{
    public static class UserSeeds
    {
        private static List<User> Users { get; set; } = new()
        {
            new User()
            {
                UserName = "vanek021",
                Email = "vanek021@yatm.ru",
            },
            new User()
            {
                UserName = "yana",
                Email = "yana@yatm.ru"
            }
        };

        private static Dictionary<User, string> UserPasswords { get; set; } = new()
        {
            { Users[0], "qwertyuiop123" },
            { Users[1], "qwertyuiop123" }
        };

        public static void Initialize(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            foreach (var user in Users)
            {
                var dbUser = userManager.FindByNameAsync(user.UserName!).GetAwaiter().GetResult();

                if (dbUser is null)
                    userManager.CreateAsync(user, UserPasswords[user]).GetAwaiter().GetResult();
            }
        }
    }
}
