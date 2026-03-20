using PortalIAAS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Infrastructure.Repositories
{
    public interface IMenuRepository
    {
        Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid objectIdUsuario);
    }
}
