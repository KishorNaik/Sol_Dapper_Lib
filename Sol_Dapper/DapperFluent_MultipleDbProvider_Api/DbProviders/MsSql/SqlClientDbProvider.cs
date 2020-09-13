using DapperFluent.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperFluent_MultipleDbProvider_Api.DbProviders.MsSql
{
    public sealed class SqlClientDbProvider : ISqlClientDbProvider
    {
        private readonly IDapperBuilder dapperBuilder = null;

        public SqlClientDbProvider(IDapperBuilder dapperBuilder)
        {
            this.dapperBuilder = dapperBuilder;
        }

        IDapperBuilder IDbProviders<SqlConnection>.DapperBuilder => dapperBuilder;

        SqlConnection IDbProviders<SqlConnection>.GetConnection()
        {
            return new SqlConnection(@"Data Source=DESKTOP-EJ69NN3\SHREE;Initial Catalog=DapperDemo;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}