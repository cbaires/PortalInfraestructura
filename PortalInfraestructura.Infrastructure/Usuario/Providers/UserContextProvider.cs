using Microsoft.AspNetCore.Components.Authorization;
using PortalInfraestructura.Application.Common.Abstractions;
using System.Security.Claims;

namespace PortalInfraestructura.Infrastructure.Usuario.Providers
{
    public class UserContextProvider(AuthenticationStateProvider authenticationStateProvider) : IUserContextProvider
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<UserContextData> ObtenerUsuarioActualAsync(CancellationToken cancellationToken = default)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user is null || !(user.Identity?.IsAuthenticated ?? false))
            {
                return new UserContextData(null, null);
            }

            var userId = ObtenerClaim(user,
                "http://schemas.microsoft.com/identity/claims/objectidentifier",
                "oid",
                ClaimTypes.NameIdentifier,
                "sub");

            var userName = ObtenerClaim(user,
                "name",
                "preferred_username",
                "upn",
                ClaimTypes.Name);

            return new UserContextData(userId, userName);
        }

        private static string? ObtenerClaim(ClaimsPrincipal user, params string[] claimTypes)
        {
            foreach (var claimType in claimTypes)
            {
                var value = user.FindFirst(claimType)?.Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }
    }
}
