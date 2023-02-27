using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Repositories.Interfaces;

namespace VendingMachine.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {

        }
    }
}
