using DapperFluent.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperFluent.Configuration
{
    public static class DapperDbProvidersServiceExtension
    {
        public static void AddDapperFluent(this IServiceCollection service)
        {
            service.AddTransient<IDapperBuilder, DapperBuilder>();
        }
    }
}