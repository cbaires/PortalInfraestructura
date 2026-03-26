using PortalInfraestructura.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PortalInfraestructura.Application.Menu.Repositories
{
    public interface IMenuRepository
    {
        Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid idUsuario, string connectionName, CancellationToken cancellationToken = default);
    }
}
