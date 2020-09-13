using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperFluent_MultipleDbProvider_Api.Models;
using DapperFluent_MultipleDbProvider_Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperFluent_MultipleDbProvider_Api.Controllers
{
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
}