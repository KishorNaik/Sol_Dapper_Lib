using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperFluent.Helpers
{
    public interface IDbProviders<TConnection> where TConnection : class, IDbConnection
    {
        TConnection GetConnection();

        IDapperBuilder DapperBuilder { get; }
    }
}