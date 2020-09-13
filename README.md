# Simple Dapper Fluent Api
[![Generic badge](https://img.shields.io/badge/Nuget-1.0.1-<COLOR>.svg)](https://www.nuget.org/packages/DapperFluent/1.0.1)

## Simple fluent api by using existing dapper's api.

### Using Nuget Package Manger:
```
PM> Install-Package DapperFluent -Version 1.0.1
```

### Using .Net CLI:
```
> dotnet add package DapperFluent --version 1.0.1
```

## Using Console Application.

### Make model class
```C#
 public class ProductModel
    {
        public decimal? ProductId { get; set; }

        public String Name { get; set; }

        public Decimal? UnitPrice { get; set; }
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
using DapperFluent.Helpers;
using System.Linq;
using System.Collections.Generic;

namespace DapperFluent.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                String connectionString = @"ConnectionString";

                // Usages of Dapper Fluent Api.
                IDapperBuilder dapperBuilder = new DapperBuilder();

                var productModel = new ProductModel()
                {
                    Name = "Chai",
                    UnitPrice = 100
                };

                //// Add Product
                var flag =
                       await
                        dapperBuilder
                        .OpenConnection(new SqlConnection(connectionString))  // Define any Database Provider
                        .Parameter(() =>
                        {
                            var dyanmicParameter = new DynamicParameters();
                            dyanmicParameter.Add("@Name", productModel.Name, DbType.String, ParameterDirection.Input);
                            dyanmicParameter.Add("@UnitPrice", productModel.UnitPrice, DbType.Decimal, ParameterDirection.Input);

                            return dyanmicParameter;
                        })
                        .Command(async (dbConnection, dynamicParameter) =>
                        {
                            var addCommand = "INSERT INTO tblProducts (Name,UnitPrice) VALUES (@Name,@UnitPrice)";

                            var status = await dbConnection?.ExecuteAsync(addCommand, param: dynamicParameter, commandType: CommandType.Text);

                            return status;
                        })
                        .ResultAsync<int?>();

                //// Select Data With Parameter

                productModel.ProductId = 1;

                var result =
                        await
                        dapperBuilder
                        .OpenConnection(new SqlConnection(connectionString))
                        .Parameter(() =>
                        {
                            // Define Sql Parameter by using Dapper DynamicParameter Class
                            var dynamicParameter = new DynamicParameters();
                            dynamicParameter.Add("@ProductId", productModel.ProductId, DbType.Decimal, ParameterDirection.Input);

                            return dynamicParameter;
                        })
                        .Command(async (dbConnection, dynamicParameter) =>
                         {
                             // Define custom query operation as per your requirement by using dapper IDbConnection list of extension methods.

                             var data =
                              await
                                  dbConnection
                                  ?.QueryFirstAsync<ProductModel>("SELECT * FROM tblProducts WHERE ProductId=@ProductId", param: dynamicParameter, commandType: CommandType.Text);

                             return data;
                         })
                        .ResultAsync<ProductModel>(); // Get Result

                // Print result
                Console.WriteLine($" Name : {result.Name} | Unit Price : {result.UnitPrice}");

                //// Select Data Without Parameter
                var resultSet =
                await
                dapperBuilder
                .OpenConnection(new SqlConnection(connectionString))
                .Command(async (dbConnection) =>
                {
                    // Define custom query operation as per your requirement by using dapper IDbConnection list of extension methods.
                    var data =
                    await
                    dbConnection
                    ?.QueryAsync<ProductModel>("SELECT * FROM tblProducts", commandType: CommandType.Text);

                    return data.ToList().AsReadOnly();
                })
                .ResultAsync<IReadOnlyCollection<ProductModel>>();

                // Print Result
                foreach (var model in resultSet)
                {
                    Console.WriteLine($" Name : {model.Name} | Unit Price : {model.UnitPrice}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

```
### Dapper Api Docs
https://dapper-tutorial.net/
