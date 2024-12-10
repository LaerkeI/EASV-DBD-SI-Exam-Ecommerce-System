using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
    }
}
