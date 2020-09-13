using DapperFluent.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperFluent_MultipleDbProvider_Api.DbProviders.MsSql
{
    public interface ISqlClientDbProvider : IDbProviders<SqlConnection>
    {
    }
}