using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Usuario.DTO
{
    public class UsuarioDto
    {
        public required string Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? IdTenant { get; set; }
        public string? Emisor { get; set; }
        public string[]? Roles { get; set; }
    }
}
