using Microsoft.Extensions.DependencyInjection;
using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Common.Behaviors;
using PortalInfraestructura.Application.Common.Dispatcher;
using System;
using System.Linq;
using System.Reflection;

namespace PortalInfraestructura.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDispatcher, ApplicationDispatcher>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionMappingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            RegistrarHandlers(services, typeof(DependencyInjection).Assembly);

            return services;
        }

        private static void RegistrarHandlers(IServiceCollection services, Assembly assembly)
        {
            var handlerInterface = typeof(IRequestHandler<,>);

            var types = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false });

            foreach (var implementationType in types)
            {
                var interfaces = implementationType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);

                foreach (var serviceType in interfaces)
                {
                    services.AddScoped(serviceType, implementationType);
                }
            }
        }
    }
}
