using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DapperFluent.FluentInterface
{
    public interface ISqlCommandDapper
    {
        Task<T> ResultAsync<T>();
    }
}
