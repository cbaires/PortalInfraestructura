using PortalIAAS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Menu.Repositories
{
    public interface IMenuRepository
    {
        public Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid idUsuario);
    }
}
