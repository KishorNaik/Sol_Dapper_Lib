using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperFluent.FluentInterface
{
    public interface ISqlDapper
    {
        ISqlConnectionDapper SqlOpenConnectionAsync(IDbConnection dbConnection);
    }
}
