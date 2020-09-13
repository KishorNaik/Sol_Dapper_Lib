using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DapperFluent.Cores
{
    public interface ICommandDapper
    {
        Task<T> ResultAsync<T>();
    }
}