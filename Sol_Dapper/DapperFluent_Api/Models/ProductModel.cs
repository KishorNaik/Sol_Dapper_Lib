using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DapperFluent_Api.Models
{
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
}