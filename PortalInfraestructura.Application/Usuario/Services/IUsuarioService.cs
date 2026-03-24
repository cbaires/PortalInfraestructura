using PortalInfraestructura.Application.Usuario.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Application.Usuario.Services
{
    public interface IUsuarioService
    {
        public Task<UsuarioDto> ObtenerInformacionUsuarioAsync();
    }
}
