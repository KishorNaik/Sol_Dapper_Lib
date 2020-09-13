using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperFluent.Cores
{
    public interface IConnectionDapper
    {
        IParameterDapper Parameter(Func<DynamicParameters> funcSqlParameter);

        IParameterDapper Parameter(Func<Task<DynamicParameters>> funcSqlParameter);

        ICommandDapper Command(Func<IDbConnection, dynamic> funcCommand);

        ICommandDapper Command<T>(Func<IDbConnection, T> funcCommand);

        ICommandDapper Command(Func<IDbConnection, Task<dynamic>> funcCommand);

        ICommandDapper Command<T>(Func<IDbConnection, Task<T>> funcCommand);
    }
}