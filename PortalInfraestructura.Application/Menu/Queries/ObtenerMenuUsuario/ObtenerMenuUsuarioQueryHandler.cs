using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Menu.DTO;
using PortalInfraestructura.Application.Menu.Ports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Menu.Queries.ObtenerMenuUsuario
{
    public class ObtenerMenuUsuarioQueryHandler(IMenuProvider menuProvider)
        : IRequestHandler<ObtenerMenuUsuarioQuery, IReadOnlyList<ModuloMenuDto>>
    {
        private readonly IMenuProvider _menuProvider = menuProvider;

        public async Task<IReadOnlyList<ModuloMenuDto>> Handle(ObtenerMenuUsuarioQuery request, CancellationToken cancellationToken)
        {
            return await _menuProvider.ObtenerMenuUsuarioAsync(request.IdUsuario, request.ConnectionName, cancellationToken);
        }
    }
}
