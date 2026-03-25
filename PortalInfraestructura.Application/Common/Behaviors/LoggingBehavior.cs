using Microsoft.Extensions.Logging;
using PortalInfraestructura.Application.Common.Abstractions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("[INICIO] Manejando {RequestName}", requestName);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next();
                stopwatch.Stop();
                _logger.LogInformation("[FIN] {RequestName} manejado con éxito ({ElapsedMilliseconds} ms)", requestName, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (System.Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "[ERROR] {RequestName} falló ({ElapsedMilliseconds} ms)", requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
