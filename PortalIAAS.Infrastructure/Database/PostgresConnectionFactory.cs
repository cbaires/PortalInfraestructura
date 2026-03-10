using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PortalIAAS.Infrastructure.Database
{
    public class PostgresConnectionFactory
    {
        public IDbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
