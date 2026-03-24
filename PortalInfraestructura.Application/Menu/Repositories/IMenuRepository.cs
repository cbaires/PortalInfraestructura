using PortalInfraestructura.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Application.Menu.Repositories
{
    public interface IMenuRepository
    {
        public Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid idUsuario);
    }
}
