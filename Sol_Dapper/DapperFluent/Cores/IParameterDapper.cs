using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperFluent.Cores
{
    public interface IParameterDapper
    {
        ICommandDapper Command(Func<IDbConnection, DynamicParameters, dynamic> funcCommand);

        ICommandDapper Command<T>(Func<IDbConnection, DynamicParameters, T> funcCommand);

        ICommandDapper Command(Func<IDbConnection, DynamicParameters, Task<dynamic>> funcCommand);

        ICommandDapper Command<T>(Func<IDbConnection, DynamicParameters, Task<T>> funcCommand);
    }
}