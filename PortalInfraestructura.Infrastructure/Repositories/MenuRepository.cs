using PortalInfraestructura.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        public async Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid objectIdUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
