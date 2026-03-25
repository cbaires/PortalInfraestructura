using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Usuario.DTO;

namespace PortalInfraestructura.Application.Usuario.Queries.ObtenerInformacionUsuario
{
    public record ObtenerInformacionUsuarioQuery : IRequest<UsuarioDto>;
}
