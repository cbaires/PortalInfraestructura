using PortalInfraestructura.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Infrastructure.Repositories
{
    public interface IMenuRepository
    {
        Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid objectIdUsuario);
    }
}
