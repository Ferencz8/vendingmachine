using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Repositories.Interfaces;

namespace VendingMachine.Tests.MockData
{
    public class ProductRepositoryFake : IProductRepository
    {
        public ProductRepositoryFake()
        {
            Products = new List<Product>();
        }
        public List<Product> Products { get; set; }
        public Task<Product> Create(Product entity)
        {
            Products.Add(entity);
            return Task.FromResult(entity);
        }

        public Task CreateInBulk(IEnumerable<Product> entities)
        {
            Products.AddRange(entities);
            return Task.FromResult(entities);
        }

        public Task Delete(object entity)
        {
            var productToBeDeletedId = entity as Product;
            var product = Products.FirstOrDefault(p => p.Id == productToBeDeletedId.Id);
            if (product != null)
            {
                Products.Remove(product);
            }
            return Task.CompletedTask;
        }

        public IQueryable<Product> GetAll(Expression<Func<Product, bool>> filterExpression = null)
        {
            return Products.AsQueryable().Where(filterExpression).AsQueryable();
        }

        public Task<Product> GetById(object id)
        {
            var product = Products.FirstOrDefault(p => p.Id == (Guid)id);
            if (product != null)
            {
                return Task.FromResult(product);
            }
            else
            {
                return Task.FromResult<Product>(null);
            }
        }

        public void Update(Product product)
        {
            var productEntity = Products.FirstOrDefault(p => p.Id == product.Id);
            if (productEntity != null)
            {
                productEntity.ProductName = product.ProductName;
                productEntity.AmountAvailable = product.AmountAvailable;
                productEntity.SellerId = product.SellerId;
                productEntity.Cost = product.Cost;
            }
        }
    }
}
