using DapperFluent.FluentInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperFluent
{
    public interface IDapperBuilder : ISqlDapper, ISqlConnectionDapper, ISqlParameterDapper, ISqlCommandDapper
    {
    }
}
