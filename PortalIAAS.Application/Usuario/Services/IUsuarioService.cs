using PortalIAAS.Application.Usuario.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Usuario.Services
{
    public interface IUsuarioService
    {
        public Task<UsuarioDto> ObtenerInformacionUsuarioAsync();
    }
}
