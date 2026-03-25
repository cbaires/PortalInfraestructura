using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Usuario.DTO;
using PortalInfraestructura.Application.Usuario.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Usuario.Queries.ObtenerInformacionUsuario
{
    public class ObtenerInformacionUsuarioQueryHandler(IUsuarioProvider usuarioProvider)
        : IRequestHandler<ObtenerInformacionUsuarioQuery, UsuarioDto>
    {
        private readonly IUsuarioProvider _usuarioProvider = usuarioProvider;

        public async Task<UsuarioDto> Handle(ObtenerInformacionUsuarioQuery request, CancellationToken cancellationToken)
        {
            return await _usuarioProvider.ObtenerInformacionUsuarioAsync();
        }
    }
}
