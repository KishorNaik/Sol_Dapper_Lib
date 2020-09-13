using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperFluent.Cores
{
    public interface IDapper
    {
        IConnectionDapper OpenConnection();

        IConnectionDapper OpenConnection(IDbConnection dbConnection);
    }
}