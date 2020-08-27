using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_orderservices.Models
{
    public interface IOrderSettings
    {
        string OrdersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
