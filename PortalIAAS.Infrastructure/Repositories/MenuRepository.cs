using PortalIAAS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        public async Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid objectIdUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
