using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Common.Exceptions;
using System.Data.Common;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Behaviors
{
    public class ExceptionMappingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (AppException)
            {
                throw;
            }
            catch (DbException ex)
            {
                throw new AppException("Ocurrió un error al acceder a los datos. Inténtalo de nuevo más tarde.", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new AppException("Ocurrió un error al comunicarse con un servicio externo. Inténtalo de nuevo más tarde.", ex);
            }
            catch (System.Exception ex)
            {
                throw new AppException("Ocurrió un error inesperado al procesar la solicitud.", ex);
            }
        }
    }
}
