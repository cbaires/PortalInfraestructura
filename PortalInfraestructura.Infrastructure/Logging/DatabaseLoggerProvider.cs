using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using PortalInfraestructura.Infrastructure.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace PortalInfraestructura.Infrastructure.Logging
{
    public sealed class DatabaseLoggerProvider(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory) : ILoggerProvider, ISupportExternalScope
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_configuration, _serviceScopeFactory, () => _scopeProvider, categoryName);
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public void Dispose()
        {
        }

        private sealed class DatabaseLogger(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, Func<IExternalScopeProvider> scopeProviderAccessor, string categoryName) : ILogger
        {
            private readonly IConfiguration _configuration = configuration;
            private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
            private readonly Func<IExternalScopeProvider> _scopeProviderAccessor = scopeProviderAccessor;
            private readonly string _categoryName = categoryName;

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
                => _scopeProviderAccessor().Push(state);

            public bool IsEnabled(LogLevel logLevel)
            {
                var minLogLevelText = _configuration["Logging:Database:LogLevel:Default"]
                    ?? _configuration["Logging:LogLevel:Default"];

                if (!Enum.TryParse<LogLevel>(minLogLevelText, true, out var minLogLevel))
                {
                    return false;
                }

                return logLevel >= minLogLevel;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                try
                {
                    var connectionName = _configuration["Logging:Database:ConnectionName"];

                    if (string.IsNullOrWhiteSpace(connectionName))
                    {
                        return;
                    }

                    var formattedMessage = formatter(state, exception);
                    var exceptionText = exception?.ToString();
                    var detalle = string.IsNullOrWhiteSpace(exceptionText)
                        ? null
                        : exceptionText;

                    var activity = Activity.Current;
                    var traceId = activity?.TraceId.ToString();
                    var spanId = activity?.SpanId.ToString();
                    var scopes = ObtenerScopes();
                    var (usuarioId, usuarioNombre) = ObtenerUsuario(scopes);

                    var context = JsonSerializer.Serialize(new
                    {
                        EventId = eventId.Id,
                        EventName = eventId.Name,
                        Categoria = _categoryName,
                        State = state?.ToString(),
                        Scopes = scopes
                    });

                    using var scope = _serviceScopeFactory.CreateScope();
                    var connectionFactory = scope.ServiceProvider.GetRequiredService<IPostgresConnectionFactory>();
                    using var connection = connectionFactory.AbrirNuevaConexion(connectionName);
                    using var command = new NpgsqlCommand("select seguridad.insertar_log(@p_level, @p_source, @p_message, @p_exception, @p_user_id, @p_user_name, @p_trace_id, @p_span_id, @p_context)", connection);
                    command.Parameters.AddWithValue("p_level", logLevel.ToString());
                    command.Parameters.AddWithValue("p_source", string.IsNullOrWhiteSpace(eventId.Name) ? _categoryName : eventId.Name);
                    command.Parameters.AddWithValue("p_message", formattedMessage);
                    command.Parameters.AddWithValue("p_exception", (object?)detalle ?? DBNull.Value);
                    command.Parameters.AddWithValue("p_user_id", (object?)usuarioId ?? DBNull.Value);
                    command.Parameters.AddWithValue("p_user_name", (object?)usuarioNombre ?? DBNull.Value);
                    command.Parameters.AddWithValue("p_trace_id", (object?)traceId ?? DBNull.Value);
                    command.Parameters.AddWithValue("p_span_id", (object?)spanId ?? DBNull.Value);
                    command.Parameters.Add("p_context", NpgsqlDbType.Jsonb).Value = context;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(
                        $"[DatabaseLoggerFallback] Source={_categoryName}; Event={eventId.Name ?? eventId.Id.ToString()}; " +
                        $"Message={formatter(state, exception)}; Error={ex}");
                }
            }

            private Dictionary<string, object?> ObtenerScopes()
            {
                var scopes = new Dictionary<string, object?>();

                _scopeProviderAccessor().ForEachScope((scopeObject, state) =>
                {
                    if (scopeObject is IEnumerable<KeyValuePair<string, object>> keyValuePairs)
                    {
                        foreach (var item in keyValuePairs)
                        {
                            state[item.Key] = item.Value;
                        }
                    }
                    else if (scopeObject is IEnumerable enumerable)
                    {
                        var index = state.Count;
                        foreach (var item in enumerable)
                        {
                            state[$"scope_{index++}"] = item;
                        }
                    }
                    else
                    {
                        state[$"scope_{state.Count}"] = scopeObject;
                    }
                }, scopes);

                return scopes;
            }

            private static (string? UserId, string? UserName) ObtenerUsuario(Dictionary<string, object?> scopes)
            {
                var userIdFromScope = ObtenerValorScope(scopes, "UserId", "userId", "userid", "UsuarioId");
                var userNameFromScope = ObtenerValorScope(scopes, "UserName", "userName", "username", "UsuarioNombre");

                if (!string.IsNullOrWhiteSpace(userIdFromScope) || !string.IsNullOrWhiteSpace(userNameFromScope))
                {
                    return (userIdFromScope, userNameFromScope);
                }

                if (Thread.CurrentPrincipal is not ClaimsPrincipal user || !(user.Identity?.IsAuthenticated ?? false))
                {
                    return (null, null);
                }

                var claims = new[]
                {
                    "http://schemas.microsoft.com/identity/claims/objectidentifier",
                    "oid",
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    "sub"
                };

                string? userId = null;
                foreach (var claim in claims)
                {
                    var value = user.FindFirst(claim)?.Value;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        userId = value;
                        break;
                    }
                }

                var userName = user.FindFirst("name")?.Value
                    ?? user.FindFirst("preferred_username")?.Value
                    ?? user.FindFirst("upn")?.Value;

                return (userId, userName);
            }

            private static string? ObtenerValorScope(Dictionary<string, object?> scopes, params string[] keys)
            {
                foreach (var key in keys)
                {
                    foreach (var item in scopes)
                    {
                        if (string.Equals(item.Key, key, StringComparison.OrdinalIgnoreCase))
                        {
                            var value = item.Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                return value;
                            }
                        }
                    }
                }

                return null;
            }
        }
    }
}
