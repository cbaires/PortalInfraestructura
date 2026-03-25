using Microsoft.Extensions.DependencyInjection;
using PortalInfraestructura.Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Application.Common.Dispatcher
{
    public class ApplicationDispatcher(IServiceProvider serviceProvider) : IApplicationDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var method = typeof(ApplicationDispatcher)
                .GetMethod(nameof(SendInternal), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(request.GetType(), typeof(TResponse));

            return (Task<TResponse>)method.Invoke(this, [request, cancellationToken])!;
        }

        private Task<TResponse> SendInternal<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>
        {
            var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse().ToList();

            RequestHandlerDelegate<TResponse> pipeline = () => handler.Handle(request, cancellationToken);

            foreach (var behavior in behaviors)
            {
                var next = pipeline;
                pipeline = () => behavior.Handle(request, next, cancellationToken);
            }

            return pipeline();
        }
    }
}
