using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperFluent.Cores;
using DapperFluent.Extension;
using DapperFluent.Helpers;

namespace DapperFluent
{
    public class DapperBuilder : IDapperBuilder
    {
        #region Declaration

        private IDbConnection _dbConnection = null;
        private DynamicParameters _dynamicParameters = null;
        private dynamic _result = null;

        #endregion Declaration

        public DapperBuilder()
        {
        }

        public DapperBuilder(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }

        #region Public Method

        IConnectionDapper IDapper.OpenConnection()
        {
            try
            {
                this._dbConnection.OpenConnection();
            }
            catch
            {
                throw;
            }

            return this;
        }

        IConnectionDapper IDapper.OpenConnection(IDbConnection dbConnection)
        {
            try
            {
                this._dbConnection = dbConnection;

                this._dbConnection.OpenConnection();
            }
            catch
            {
                throw;
            }

            return this;
        }

        ICommandDapper IConnectionDapper.Command(Func<IDbConnection, dynamic> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection);
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        public ICommandDapper Command<T>(Func<IDbConnection, T> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection);
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        public ICommandDapper Command(Func<IDbConnection, Task<dynamic>> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        public ICommandDapper Command<T>(Func<IDbConnection, Task<T>> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        IParameterDapper IConnectionDapper.Parameter(Func<DynamicParameters> funcSqlParameter)
        {
            try
            {
                _dynamicParameters = funcSqlParameter();
            }
            catch
            {
                throw;
            }

            return this;
        }

        IParameterDapper IConnectionDapper.Parameter(Func<Task<DynamicParameters>> funcSqlParameter)
        {
            try
            {
                _dynamicParameters = funcSqlParameter().GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }

            return this;
        }

        ICommandDapper IParameterDapper.Command(Func<IDbConnection, DynamicParameters, dynamic> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection, _dynamicParameters);
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        ICommandDapper IParameterDapper.Command<T>(Func<IDbConnection, DynamicParameters, T> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection, _dynamicParameters);
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        ICommandDapper IParameterDapper.Command(Func<IDbConnection, DynamicParameters, Task<dynamic>> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection, _dynamicParameters).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        ICommandDapper IParameterDapper.Command<T>(Func<IDbConnection, DynamicParameters, Task<T>> funcCommand)
        {
            try
            {
                _result = funcCommand(_dbConnection, _dynamicParameters).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection?.CloseConnection();
            }

            return this;
        }

        Task<T> ICommandDapper.ResultAsync<T>()
        {
            return Task.Run(() =>
            {
                try
                {
                    return (T)(dynamic)_result;
                }
                catch
                {
                    throw;
                }
            });
        }

        #endregion Public Method
    }
}