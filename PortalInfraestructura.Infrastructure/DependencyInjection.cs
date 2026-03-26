using Microsoft.Extensions.DependencyInjection;
using PortalInfraestructura.Application.Menu.Ports;
using PortalInfraestructura.Application.Menu.Repositories;
using PortalInfraestructura.Application.Usuario.Ports;
using PortalInfraestructura.Infrastructure.Database;
using PortalInfraestructura.Infrastructure.Menu.Providers;
using PortalInfraestructura.Infrastructure.Menu.Repositories;
using PortalInfraestructura.Infrastructure.Usuario.Providers;

namespace PortalInfraestructura.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPostgresConnectionFactory, PostgresConnectionFactory>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuProvider, MenuProvider>();
            services.AddScoped<IUsuarioProvider, UsuarioProvider>();
            return services;
        }
    }
}
