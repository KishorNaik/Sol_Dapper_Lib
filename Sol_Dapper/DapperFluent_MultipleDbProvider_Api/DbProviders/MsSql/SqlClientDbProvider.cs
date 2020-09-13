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
            return new SqlConnection(@"ConnectionString");
        }
    }
}