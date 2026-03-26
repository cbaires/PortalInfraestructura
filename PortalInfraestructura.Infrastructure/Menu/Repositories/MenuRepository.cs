using Dapper;
using Npgsql;
using PortalInfraestructura.Application.Common.Exceptions;
using PortalInfraestructura.Application.Menu.Repositories;
using PortalInfraestructura.Domain.Models;
using PortalInfraestructura.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortalInfraestructura.Infrastructure.Menu.Repositories
{
    public class MenuRepository(IPostgresConnectionFactory connectionFactory) : IMenuRepository
    {
        private readonly IPostgresConnectionFactory _connectionFactory = connectionFactory;
        private const int _maxRetries = 3;

        public async Task<IReadOnlyList<ModuloMenu>> ObtenerMenuUsuarioAsync(Guid idUsuario, string connectionName, CancellationToken cancellationToken = default)
        {
            for (var attempt = 1; attempt <= _maxRetries; attempt++)
            {
                await using var conexion = await _connectionFactory.AbrirNuevaConexionAsync(connectionName, cancellationToken);
                await using var transaction = await conexion.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

                try
                {
                    var command = new CommandDefinition(
                        commandText: "select * from seguridad.consultar_menu_usuario(@usuario_id)",
                        parameters: new { usuario_id = idUsuario },
                        transaction: transaction,
                        cancellationToken: cancellationToken);

                    var rows = await conexion.QueryAsync(command);
                    var resultado = rows
                        .Select(row => (IDictionary<string, object>)row)
                        .Select(row => new ModuloMenu
                        {
                            Id = ObtenerIntNullable(row, "id", "id_modulo"),
                            Nivel = ObtenerIntNullable(row, "nivel"),
                            Nombre = ObtenerStringNullable(row, "nombre"),
                            Icono = ObtenerStringNullable(row, "icono"),
                            Url = ObtenerStringNullable(row, "url"),
                            IdModuloPadre = ObtenerIntNullable(row, "id_modulo_padre", "id_padre")
                        })
                        .ToList();

                    await transaction.CommitAsync(cancellationToken);

                    return resultado;
                }
                catch (PostgresException ex) when (EsErrorDeAislamiento(ex) && attempt < _maxRetries)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    await Task.Delay(TimeSpan.FromMilliseconds(150 * attempt), cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            throw new AppException("No se pudo consultar el menú del usuario debido a conflictos de concurrencia en la base de datos.");
        }

        private static bool EsErrorDeAislamiento(PostgresException ex)
        {
            return ex.SqlState is PostgresErrorCodes.SerializationFailure or PostgresErrorCodes.DeadlockDetected;
        }

        private static int? ObtenerIntNullable(IDictionary<string, object> row, params string[] columns)
        {
            var value = ObtenerValorNullable(row, columns);
            if (value is null)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }

        private static string? ObtenerStringNullable(IDictionary<string, object> row, params string[] columns)
        {
            var value = ObtenerValorNullable(row, columns);
            if (value is null)
            {
                return null;
            }

            return Convert.ToString(value);
        }

        private static object? ObtenerValorNullable(IDictionary<string, object> row, params string[] columns)
        {
            foreach (var column in columns)
            {
                var key = row.Keys.FirstOrDefault(k => string.Equals(k, column, StringComparison.OrdinalIgnoreCase));
                if (key is not null)
                {
                    var value = row[key];
                    if (value == DBNull.Value)
                    {
                        return null;
                    }

                    return value;
                }
            }

            return null;
        }
    }
}
