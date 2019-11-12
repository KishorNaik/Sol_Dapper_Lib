using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperFluent.Extension
{
    public static class DbConnectionExtension
    {
        public static async Task OpenConnectionAsync(this IDbConnection dbConnection)
        {
            
                await Task.Run(() =>
                {
                    try
                    {
                        if (dbConnection?.State == ConnectionState.Closed || dbConnection?.State == ConnectionState.Broken)
                        {
                            dbConnection?.Open();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                });
          
        }

        public static async Task CloseConnectionAsync(this IDbConnection dbConnection)
        {
            
                await Task.Run(() => {

                    try
                    {
                        if (dbConnection?.State == ConnectionState.Open)
                        {
                            dbConnection?.Close();
                            dbConnection.Dispose();
                            dbConnection = null;
                        }
                    }
                    catch
                    {
                        throw;
                    }

                    

                });
           
        }
    }
}
