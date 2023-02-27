using System.Runtime.CompilerServices;
using VendingMachine.API.Models;
using VendingMachine.BLL.Exceptions;
using VendingMachine.BLL.Interfaces;
using VendingMachine.DAL;
using VendingMachine.DAL.Entities;

namespace VendingMachine.BLL
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProduct(ProductRequest product, string currentUserId)
        {
            var producyEntity = new Product()
            {
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                ProductName = product.ProductName,
                SellerId = currentUserId
            };
            await _unitOfWork.ProductRepository.Create(producyEntity);
            await _unitOfWork.Save();
        }

        public async Task DeleteProduct(Guid id, string currentUserId)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetById(id);
            if (productEntity == null)
            {
                throw new ProductNotFoundException(id);
            }

            if(productEntity.SellerId != currentUserId)
            {
                throw new UnauthorizedOperationException(id);
            }

            await _unitOfWork.ProductRepository.Delete(productEntity);
            await _unitOfWork.Save();
        }

        public List<Product> GetAllProducts()
        {
            return _unitOfWork.ProductRepository.GetAll().ToList();
        }

        public async Task<Product> GetProduct(Guid id)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetById(id);
            if (productEntity == null)
            {
                throw new ProductNotFoundException(id);
            }
            return productEntity;
        }

        public async Task UpdateProduct(Product product, string currentUserId)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetById(product.Id);
            if (productEntity == null)
            {
                throw new ProductNotFoundException(product.Id);
            }

            if (productEntity.SellerId != currentUserId)
            {
                throw new UnauthorizedOperationException(product.Id);
            }
            productEntity.ProductName = product.ProductName;
            productEntity.AmountAvailable = product.AmountAvailable;
            productEntity.Cost = product.Cost;
            _unitOfWork.ProductRepository.Update(productEntity);
            await _unitOfWork.Save();
        }
    }
}