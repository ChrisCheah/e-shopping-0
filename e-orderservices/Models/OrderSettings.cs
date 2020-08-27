using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_orderservices.Models
{
    public class OrderSettings:IOrderSettings
    {

        public string OrdersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
