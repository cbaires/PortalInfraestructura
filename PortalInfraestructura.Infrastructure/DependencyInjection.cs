using Microsoft.Extensions.DependencyInjection;
using PortalInfraestructura.Application.Usuario.Ports;
using PortalInfraestructura.Infrastructure.Services.Usuario;

namespace PortalInfraestructura.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioProvider, UsuarioService>();
            return services;
        }
    }
}
