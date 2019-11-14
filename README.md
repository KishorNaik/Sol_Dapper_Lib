# Simple Dapper Fluent Api
[![Generic badge](https://img.shields.io/badge/Nuget-1.0.0-<COLOR>.svg)](https://www.nuget.org/packages/DapperFluent/)

## Simple fluent api by using existing dapper's api.

### Using Nuget Package Manger:
```
PM> Install-Package Dapper -Version 2.0.30
PM> Install-Package DapperFluent -Version 1.0.0
```

### Using .Net CLI:
```
> dotnet add package Dapper --version 2.0.30
> dotnet add package DapperFluent --version 1.0.0
```

### Usage.

### Make model class
```C#
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperFluent.Client
{
    public class UserModel
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }
    }
}

```

### Dapper Fluent Usages
```C#
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
           
                Task.Run(async () => {

                    try
                    {
                        // Create an instance of user Model Class and bind data.
                        var userModelObj = new UserModel()
                        {
                            FirstName = "Kishor",
                            LastName = "Naik"
                        };

                        // Usages of Dapper Fluent Api.
                        IDapperBuilder dapperBuilder = new DapperBuilder();

                        int? flag =
                               await
                               dapperBuilder
                              ?.SqlOpenConnectionAsync(new SqlConnection("Connection String")) // Define any Database Provider
                              ?.SqlParameter(() =>
                              {
                                  // Define Sql Parameter by using Dapper DynamicParameter Class
                                  var dynamicParameter = new DynamicParameters();
                                  dynamicParameter.Add("@FirstName", userModelObj?.FirstName , DbType.String, ParameterDirection.Input);
                                  dynamicParameter.Add("@LastName", userModelObj.LastName, DbType.String, ParameterDirection.Input);

                                  return dynamicParameter;

                              })
                              ?.SqlCommandAsync<int?>(async (leDbConnection, leDynamicParameter) =>
                              {
                                  // Define custom query operation as per your requirement by using dapper IDbConnection list of extension methods.

                                  int? result =  
                                    await
                                      leDbConnection
                                      ?.ExecuteAsync("uspAddUsers", param: leDynamicParameter, commandType: CommandType.StoredProcedure);

                                  return result;

                              })
                              ?.ResultAsync<int?>(); // Get Result

                        Console.WriteLine((flag >= 1) ? "Data successfully save." : "something went wrong");
                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }

                }).Wait();
        }
    }
}


```
### Dapper Api Docs
https://dapper-tutorial.net/
