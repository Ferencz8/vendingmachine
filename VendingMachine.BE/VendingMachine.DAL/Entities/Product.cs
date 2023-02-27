using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.DAL.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public int AmountAvailable { get; set; }

        public int Cost { get; set; }

        public string ProductName { get; set; }

        public string SellerId { get; set; }
    }
}
