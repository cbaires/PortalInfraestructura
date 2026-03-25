using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Abstractions
{
    public interface IApplicationDispatcher
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
