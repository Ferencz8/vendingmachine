using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BLL.Models
{
    public class BuyRequest
    {
        public Guid ProductId { get; set; }

        public int AmountOfProducts { get; set; }
    }
}
