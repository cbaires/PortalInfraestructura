using Npgsql;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Infrastructure.Database
{
    public interface IPostgresConnectionFactory
    {
        Task<NpgsqlConnection> AbrirNuevaConexionAsync(string nombreCadenaConexion, CancellationToken cancellationToken = default);
    }
}
