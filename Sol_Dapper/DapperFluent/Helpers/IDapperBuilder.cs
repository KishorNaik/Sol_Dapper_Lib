using DapperFluent.Cores;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperFluent.Helpers
{
    public interface IDapperBuilder : IDapper, IConnectionDapper, IParameterDapper, ICommandDapper
    {
    }
}