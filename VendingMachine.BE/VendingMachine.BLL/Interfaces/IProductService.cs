using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.API.Models;
using VendingMachine.DAL.Entities;

namespace VendingMachine.BLL.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();

        Task<Product> GetProduct(Guid id);

        Task AddProduct(ProductRequest product, string currentUserId);

        Task UpdateProduct(Product product, string currentUserId);

        Task DeleteProduct(Guid id, string currentUserId);
    }
}
