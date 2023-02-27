using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BLL.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public Guid ProductId { get; set; }
        public ProductNotFoundException(Guid productId) : base($"Product with id: {productId} not found.")
        {
            ProductId = productId;
        }
    }
}
