using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}
