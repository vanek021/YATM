using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATM.Core.Models
{
    public class AuthenticationStateAccessor : IAuthenticationStateAccessor
    {
        private AuthenticationStateProvider _authenticationStateProvider;
        public void SetProvider(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }
        public AuthenticationState AuthenticationState => _authenticationStateProvider?.GetAuthenticationStateAsync().GetAwaiter().GetResult();
    }

    public interface IAuthenticationStateAccessor
    {
        AuthenticationState AuthenticationState { get; }
    }
}
