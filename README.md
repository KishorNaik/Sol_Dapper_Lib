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

### Define product model class.
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
#### [Program.cs](https://github.com/KishorNaik/Sol_Dapper_Lib/blob/master/Sol_Dapper/DapperFluent.Client/Program.cs)

## Using Asp.net core web api or MVC project template(Core 3.1). 
### Note : If you want to work on a single database provider such as Ms-Sql or My-Sql or Oracle on same api project solution then use the following code
For this demo, i am going to use SqlConnection db client provider.

#### Step 1
Go to StartUp.cs file, add the following service on **ConfigureServices** method.
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    // Get Connection String from appsetting.json file.
    services.AddDapperFluent(new SqlConnection("ConnectionString"));
}
```

#### Step 2
Define Product model class for passing data between repository and controller.

```C#
[DataContract]
public class ProductModel
{
    [DataMember(EmitDefaultValue = false)]
    public decimal? ProductId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public String Name { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public Decimal? UnitPrice { get; set; }
}
```

#### Step 3
Define a IProductRepository interface and ProductRepository class and inject **IDapperBuilder** interface on a repository constructor.
```C#
public interface IProductRepository
{
    Task<ProductModel> GetProductDataByIdAsync(decimal? id);

    Task<IReadOnlyCollection<ProductModel>> GetProductDataAsync();
}

public sealed class ProductRepository : IProductRepository
{
    private readonly IDapperBuilder dapperBuilder = null;

    public ProductRepository(IDapperBuilder dapperBuilder)
    {
        this.dapperBuilder = dapperBuilder;
    }

    async Task<IReadOnlyCollection<ProductModel>> IProductRepository.GetProductDataAsync()
    {
        try
        {
            var resultSet =
                   await
                   dapperBuilder
                   ?.OpenConnection()
                   ?.Command(async (dbConnection) =>
                   {
                       return
                            (await
                            dbConnection
                            ?.QueryAsync<ProductModel>("SELECT * FROM tblProducts", commandType: CommandType.Text)
                            )
                            ?.ToList()
                            ?.AsReadOnly();
                   })
                   ?.ResultAsync<IReadOnlyCollection<ProductModel>>();

            return resultSet;
        }
        catch
        {
            throw;
        }
    }

    async Task<ProductModel> IProductRepository.GetProductDataByIdAsync(decimal? id)
    {
        try
        {
            var result =
                   await
                   dapperBuilder
                   ?.OpenConnection()
                   ?.Parameter(() =>
                   {
                       var dynamicParameters = new DynamicParameters();
                       dynamicParameters.Add("ProductId", id, DbType.Decimal, ParameterDirection.Input);

                       return dynamicParameters;
                   })
                   ?.Command(async (dbConnection, dynamicParameter) =>
                   {
                       return
                            await
                            dbConnection
                            ?.QueryFirstAsync<ProductModel>("SELECT * FROM tblProducts WHERE ProductId=@ProductId", param: dynamicParameter, commandType: CommandType.Text);
                   })
                   ?.ResultAsync<ProductModel>();

            return result;
        }
        catch
        {
            throw;
        }
    }
}
```
dapper builder objects have four fluent methods.
* OpenConnection() : to open sql connection.
* Parameter() : to define sql parameters using **DynamicParameter** dapper api.
* Command() : to define sql commands using dapper api.
* ResultAsync() : to get results from command.

#### Step 4
Create a ProductController, add the following code.
```C#
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository productRepository = null;

    public ProductsController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    [HttpPost("getproducts")]
    public async Task<IActionResult> GetProductsAsync()
    {
        try
        {
            var data = await productRepository?.GetProductDataAsync();

            return base.Ok(data);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("getproductsbyid")]
    public async Task<IActionResult> GetProductsByIdAsync([FromBody] ProductModel productModel)
    {
        try
        {
            if (productModel.ProductId == null) return base.BadRequest();

            var data = await productRepository?.GetProductDataByIdAsync(productModel.ProductId);

            return base.Ok(data);
        }
        catch
        {
            throw;
        }
    }
}

```

#### Step 5
Go to StartUp.cs file, register the following transient service for specified type on **ConfigureServices** method.
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddDapperFluent(new SqlConnection("ConnectionString"));
    // Register Service DI
    services.AddTransient<IProductRepository, ProductRepository>();
}
```
Source Code : https://github.com/KishorNaik/Sol_Dapper_Lib/tree/master/Sol_Dapper/DapperFluent_Api

## Using Asp.net core web api or MVC project template(Core 3.1). 
### Note : If you want to work on a multiple database provider such as Ms-Sql and My-Sql and Oracle on same api project solution then use the following code
For this demo, i am going to use SqlConnection db client provider.

#### Step 1
Go to StartUp.cs file, add the following service on **ConfigureServices** method.
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    
    // Register Dapper Fluent
    services.AddDapperFluent();
}
```

#### Step 2
Define Product model class for passing data between repository and controller.

```C#
[DataContract]
public class ProductModel
{
    [DataMember(EmitDefaultValue = false)]
    public decimal? ProductId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public String Name { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public Decimal? UnitPrice { get; set; }
}
```
#### Step 3
Define multiple client db provider class. In this demo, i am going to use **SqlConnection** client provider.
```C#
public interface ISqlClientDbProvider : IDbProviders<SqlConnection>
{
}

public sealed class SqlClientDbProvider : ISqlClientDbProvider
{
    private readonly IDapperBuilder dapperBuilder = null;

    public SqlClientDbProvider(IDapperBuilder dapperBuilder)
    {
        this.dapperBuilder = dapperBuilder;
    }

    IDapperBuilder IDbProviders<SqlConnection>.DapperBuilder => dapperBuilder;

    SqlConnection IDbProviders<SqlConnection>.GetConnection()
    {
        return new SqlConnection(@"ConnectionString");
    }
}
```
Implement **IDbProviders** interface by passing db client as generic type. If we want to use MySql or Oracle db client then define respective db client as generic type but make sure that it would create a new inteface and class for different db providers.


#### Step 4
Define a IProductRepository interface and ProductRepository class and inject **ISqlClientDbProvider** interface on a repository constructor.
```C#
public interface IProductRepository
{
    Task<ProductModel> GetProductDataByIdAsync(decimal? id);

    Task<IReadOnlyCollection<ProductModel>> GetProductDataAsync();
}

public sealed class ProductRepository : IProductRepository
{
  private readonly ISqlClientDbProvider sqlClientDbProvider = null;

  public ProductRepository(ISqlClientDbProvider sqlClientDbProvider)
  {
      this.sqlClientDbProvider = sqlClientDbProvider;
  }

  async Task<IReadOnlyCollection<ProductModel>> IProductRepository.GetProductDataAsync()
  {
      try
      {
          var resultSet =
                 await
                 sqlClientDbProvider
                 .DapperBuilder
                 ?.OpenConnection(sqlClientDbProvider.GetConnection())
                 ?.Command(async (dbConnection) =>
                 {
                     return
                          (await
                          dbConnection
                          ?.QueryAsync<ProductModel>("SELECT * FROM tblProducts", commandType: CommandType.Text)
                          )
                          ?.ToList()
                          ?.AsReadOnly();
                 })
                 ?.ResultAsync<IReadOnlyCollection<ProductModel>>();

          return resultSet;
      }
      catch
      {
          throw;
      }
  }

  async Task<ProductModel> IProductRepository.GetProductDataByIdAsync(decimal? id)
  {
      try
      {
          var result =
                 await
                 sqlClientDbProvider
                 .DapperBuilder
                 ?.OpenConnection(sqlClientDbProvider.GetConnection())
                 ?.Parameter(() =>
                 {
                     var dynamicParameters = new DynamicParameters();
                     dynamicParameters.Add("ProductId", id, DbType.Decimal, ParameterDirection.Input);

                     return dynamicParameters;
                 })
                 ?.Command(async (dbConnection, dynamicParameter) =>
                 {
                     return
                          await
                          dbConnection
                          ?.QueryFirstAsync<ProductModel>("SELECT * FROM tblProducts WHERE ProductId=@ProductId", param: dynamicParameter, commandType: CommandType.Text);
                 })
                 ?.ResultAsync<ProductModel>();

          return result;
      }
      catch
      {
          throw;
      }
  }
}
```
**Note : Here you can inject many db client providers on a single repository.**

#### Step 5
Create a ProductController, add the following code.
```C#
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository productRepository = null;

    public ProductsController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    [HttpPost("getproducts")]
    public async Task<IActionResult> GetProductsAsync()
    {
        try
        {
            var data = await productRepository?.GetProductDataAsync();

            return base.Ok(data);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("getproductsbyid")]
    public async Task<IActionResult> GetProductsByIdAsync([FromBody] ProductModel productModel)
    {
        try
        {
            if (productModel.ProductId == null) return base.BadRequest();

            var data = await productRepository?.GetProductDataByIdAsync(productModel.ProductId);

            return base.Ok(data);
        }
        catch
        {
            throw;
        }
    }
}

```

#### Step 6
Go to StartUp.cs file, register the following transient service for specified type on **ConfigureServices** method.
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddDapperFluent();
    // Register Service DI
    services.AddTransient<ISqlClientDbProvider, SqlClientDbProvider>();
    services.AddTransient<IProductRepository, ProductRepository>();
}
```
Source Code : https://github.com/KishorNaik/Sol_Dapper_Lib/tree/master/Sol_Dapper/DapperFluent_MultipleDbProvider_Api


### Dapper Api Docs
https://dapper-tutorial.net/
