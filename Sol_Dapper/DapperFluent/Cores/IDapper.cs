using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperFluent.Cores
{
    public interface IDapper
    {
        IConnectionDapper OpenConnection(IDbConnection dbConnection);
    }
}