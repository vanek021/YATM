using Microsoft.AspNetCore.Identity;
using YATM.Core.Models;
using YATM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace YATM.Services
{
    public class ApplicationContext
    {
        public User CurrentUser { get; init; }

        public ApplicationContext(IAuthenticationStateAccessor authenticationStateProvider, UserManager<User> userManager) // TODO: сделать кастомный манагер
        {
            var state = authenticationStateProvider.AuthenticationState;
            if (state == null)
                return;

            if (!state.User.Identity.IsAuthenticated)
                return;

            CurrentUser = userManager.Users
                .Where(x => x.UserName == state.User.Identity.Name)
                .FirstOrDefault()!;
        }
    }
}
