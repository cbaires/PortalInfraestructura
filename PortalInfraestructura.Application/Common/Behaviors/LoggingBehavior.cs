using Microsoft.Extensions.Logging;
using PortalInfraestructura.Application.Common.Abstractions;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IUserContextProvider userContextProvider)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;
        private readonly IUserContextProvider _userContextProvider = userContextProvider;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var eventId = new EventId(1000, requestName);
            var userContext = await _userContextProvider.ObtenerUsuarioActualAsync(cancellationToken);

            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = requestName,
                ["UserId"] = userContext.UserId ?? string.Empty,
                ["UserName"] = userContext.UserName ?? string.Empty
            });

            _logger.LogInformation(eventId, "[INICIO] Manejando {RequestName}", requestName);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next();
                stopwatch.Stop();
                _logger.LogInformation(eventId, "[FIN] {RequestName} manejado con éxito ({ElapsedMilliseconds} ms)", requestName, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (System.Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(eventId, ex, "[ERROR] {RequestName} falló ({ElapsedMilliseconds} ms)", requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
