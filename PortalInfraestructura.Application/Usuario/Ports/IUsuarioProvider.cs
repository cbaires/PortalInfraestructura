using PortalInfraestructura.Application.Usuario.DTO;

namespace PortalInfraestructura.Application.Usuario.Ports
{
    public interface IUsuarioProvider
    {
        Task<UsuarioDto> ObtenerInformacionUsuarioAsync();
    }
}
