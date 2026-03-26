using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Menu.DTO;
using System;
using System.Collections.Generic;

namespace PortalInfraestructura.Application.Menu.Queries.ObtenerMenuUsuario
{
    public record ObtenerMenuUsuarioQuery(Guid? IdUsuario = null, string? ConnectionName = null) : IRequest<IReadOnlyList<ModuloMenuDto>>;
}
