using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Abstractions
{
    public sealed record UserContextData(string? UserId, string? UserName);

    public interface IUserContextProvider
    {
        Task<UserContextData> ObtenerUsuarioActualAsync(CancellationToken cancellationToken = default);
    }
}
