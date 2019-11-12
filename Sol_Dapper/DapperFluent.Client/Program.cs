using System;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Diagnostics;

namespace DapperFluent.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

                Task.Run(async () => {

                    try
                    {
                        var userModelObj = new UserModel()
                        {
                            FirstName = "Sam",
                            LastName = "Naik"
                        };

                        Stopwatch stopwatch = new Stopwatch();

                        stopwatch.Start();

                        IDapperBuilder dapperBuilder = new DapperBuilder();
                        int? flag =
                               await
                               dapperBuilder
                              ?.SqlOpenConnectionAsync(new SqlConnection(@"Data Source=DESKTOP-MOL1H66\IDEATORS;Initial Catalog=Ideators;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                              ?.SqlParameter(() =>
                              {

                                  var dynamicParameter = new DynamicParameters();
                                  dynamicParameter.Add("@FirstName", userModelObj?.FirstName , DbType.String, ParameterDirection.Input);
                                  dynamicParameter.Add("@LastName", userModelObj.LastName, DbType.String, ParameterDirection.Input);

                                  return dynamicParameter;

                              })
                              ?.SqlCommandAsync<int?>(async (leDbConnection, leDynamicParameter) =>
                              {

                                  int? result =  
                                    await
                                      leDbConnection
                                      ?.ExecuteAsync("uspAddUsers", param: leDynamicParameter, commandType: CommandType.StoredProcedure);

                                  return result;

                              })
                              ?.ResultAsync<int?>();

                        Console.WriteLine((flag >= 1) ? "Data successfully save." : "something went wrong");
                        stopwatch.Stop();

                        Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Hello");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }

                }).Wait();
        }
    }
}
