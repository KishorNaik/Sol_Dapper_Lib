using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using DapperFluent_MultipleDbProvider_Api.Models;
using DapperFluent_MultipleDbProvider_Api.DbProviders.MsSql;

namespace DapperFluent_MultipleDbProvider_Api.Repository
{
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
}