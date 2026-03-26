using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PortalInfraestructura.Application.Common.Abstractions;
using PortalInfraestructura.Application.Menu.Ports;
using PortalInfraestructura.Application.Menu.Repositories;
using PortalInfraestructura.Application.Usuario.Ports;
using PortalInfraestructura.Infrastructure.Database;
using PortalInfraestructura.Infrastructure.Logging;
using PortalInfraestructura.Infrastructure.Menu.Providers;
using PortalInfraestructura.Infrastructure.Menu.Repositories;
using PortalInfraestructura.Infrastructure.Usuario.Providers;

namespace PortalInfraestructura.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerProvider, DatabaseLoggerProvider>();
            services.AddScoped<IPostgresConnectionFactory, PostgresConnectionFactory>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuProvider, MenuProvider>();
            services.AddScoped<IUsuarioProvider, UsuarioProvider>();
            services.AddScoped<IUserContextProvider, UserContextProvider>();
            return services;
        }
    }
}
