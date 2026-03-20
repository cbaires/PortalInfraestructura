using Microsoft.AspNetCore.Components.Authorization;
using PortalIAAS.Application.Common.Exceptions;
using PortalIAAS.Application.Usuario.DTO;
using PortalIAAS.Application.Usuario.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Infrastructure.Services.Usuario
{
    public class UsuarioService(AuthenticationStateProvider authStateProvider) : IUsuarioService
    {
        private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

        public async Task<UsuarioDto> ObtenerInformacionUsuarioAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user == null || !(user.Identity?.IsAuthenticated ?? false))
            {
                throw new UsuarioNoAutenticadoException("No se encontró un usuario autenticado");
            }

            string ObtenerClaim(params string[] tipos)
            {
                foreach (var tipo in tipos)
                {
                    var valor = user.FindFirst(tipo)?.Value;
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        return valor;
                    }
                }
                return string.Empty;
            }

            var id = ObtenerClaim("oid");

            if (string.IsNullOrEmpty(id))
            {
                throw new ClaimsUsuarioException("No se encontró el claim 'oid' para identificar al usuario");
            }

            var roles = user.FindAll("roles").Select(r => r.Value).ToList();

            return new UsuarioDto
            {
                Id = id,
                NombreUsuario = ObtenerClaim("preferred_username", "upn"),
                Correo = ObtenerClaim("email"),
                NombreCompleto = ObtenerClaim("name"),
                Nombre = ObtenerClaim("given_name"),
                Apellido = ObtenerClaim("family_name"),
                IdTenant = ObtenerClaim("tid"),
                Emisor = ObtenerClaim("iss"),
                Roles = [.. roles]
            };
        }
    }
}
