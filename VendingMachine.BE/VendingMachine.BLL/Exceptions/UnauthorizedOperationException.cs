using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BLL.Exceptions
{
    public class UnauthorizedOperationException : Exception
    {
        public Guid ProductId { get; set; }
        public UnauthorizedOperationException(Guid productId) : base($"Product with id {productId} was not added by the same use who wants to modify it.")
        {
            ProductId = productId;
        }
    }
}