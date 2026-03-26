using PortalInfraestructura.Application.Menu.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Menu.Ports
{
    public interface IMenuProvider
    {
        Task<IReadOnlyList<ModuloMenuDto>> ObtenerMenuUsuarioAsync(Guid? idUsuario = null, string? connectionName = null, CancellationToken cancellationToken = default);
    }
}
