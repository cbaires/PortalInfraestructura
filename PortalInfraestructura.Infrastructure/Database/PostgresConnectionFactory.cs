using Microsoft.Extensions.Configuration;
using Npgsql;
using PortalInfraestructura.Application.Common.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Infrastructure.Database
{
    public class PostgresConnectionFactory(IConfiguration configuration) : IPostgresConnectionFactory
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<NpgsqlConnection> AbrirNuevaConexionAsync(string nombreCadenaConexion, CancellationToken cancellationToken = default)
        {
            var cadenaConexion = _configuration.GetConnectionString(nombreCadenaConexion);

            if (string.IsNullOrWhiteSpace(cadenaConexion))
            {
                throw new AppException($"No se encontró la cadena de conexión '{nombreCadenaConexion}'.");
            }

            var conexion = new NpgsqlConnection(cadenaConexion);
            await conexion.OpenAsync(cancellationToken);
            return conexion;
        }
    }
}
