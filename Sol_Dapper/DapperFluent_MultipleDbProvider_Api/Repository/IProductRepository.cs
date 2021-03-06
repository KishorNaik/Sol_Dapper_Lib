﻿using DapperFluent_MultipleDbProvider_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperFluent_MultipleDbProvider_Api.Repository
{
    public interface IProductRepository
    {
        Task<ProductModel> GetProductDataByIdAsync(decimal? id);

        Task<IReadOnlyCollection<ProductModel>> GetProductDataAsync();
    }
}