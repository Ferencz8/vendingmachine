using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BLL.Exceptions
{
    public class InsuficientFundsException : Exception
    {
        public Guid ProductId { get; set; }
        public InsuficientFundsException(Guid productId) : base($"User does not have enough money to purchase product with id {productId}.")
        {
            ProductId = productId;
        }
    }
}