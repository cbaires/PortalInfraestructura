using PortalInfraestructura.Application.Common.Exceptions;
using PortalInfraestructura.Application.Menu.DTO;
using PortalInfraestructura.Application.Menu.Ports;
using PortalInfraestructura.Application.Menu.Repositories;
using PortalInfraestructura.Application.Usuario.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Infrastructure.Menu.Providers
{
    public class MenuProvider(IMenuRepository menuRepository, IUsuarioProvider usuarioProvider) : IMenuProvider
    {
        private readonly IMenuRepository _menuRepository = menuRepository;
        private readonly IUsuarioProvider _usuarioProvider = usuarioProvider;

        public async Task<IReadOnlyList<ModuloMenuDto>> ObtenerMenuUsuarioAsync(Guid? idUsuario = null, string? connectionName = null, CancellationToken cancellationToken = default)
        {
            var usuarioId = idUsuario ?? await ObtenerIdUsuarioActualAsync();
            var menus = await _menuRepository.ObtenerMenuUsuarioAsync(usuarioId, connectionName ?? "Postgres", cancellationToken);

            return menus
                .Select(m => new ModuloMenuDto
                {
                    Id = m.Id,
                    Nivel = m.Nivel,
                    Nombre = m.Nombre,
                    Icono = m.Icono,
                    Url = m.Url,
                    IdModuloPadre = m.IdModuloPadre
                })
                .ToList();
        }

        private async Task<Guid> ObtenerIdUsuarioActualAsync()
        {
            var usuario = await _usuarioProvider.ObtenerInformacionUsuarioAsync();

            if (!Guid.TryParse(usuario.Id, out var usuarioId))
            {
                throw new AppException($"El id de usuario '{usuario.Id}' no tiene un formato GUID válido.");
            }

            return usuarioId;
        }
    }
}
