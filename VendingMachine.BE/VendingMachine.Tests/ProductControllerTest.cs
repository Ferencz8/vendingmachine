using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.API.Controllers;
using VendingMachine.BLL;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL;
using VendingMachine.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Exceptions;
using VendingMachine.API.Models;

namespace VendingMachine.Tests
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public async Task AddProduct_Should_AddProductIfDataIsValid()
        {
            var product = new ProductRequest()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);
            
            
            var productService = new ProductService(unitOfWorkMoq.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new ProductController(productService);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Add(product);

            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task AddProduct_Should_Return_OkResult()
        {
            var product = new ProductRequest()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var productServiceMoq = new Mock<IProductService>();

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new ProductController(productServiceMoq.Object);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Add(product);

            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateProduct_Should_Return_OkResult()
        {
            var product = new Product()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var productServiceMoq = new Mock<IProductService>();

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new ProductController(productServiceMoq.Object);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };

            /// Act
            var result = await sut.Update(product);

            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task RemoveProduct_Should_Return_OkResult()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var productServiceMoq = new Mock<IProductService>();

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new ProductController(productServiceMoq.Object);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };

            /// Act
            var result = await sut.Remove(product.Id);

            Assert.IsNotNull(result);
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetProduct_Should_Return_OkObjectResult()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var productServiceMoq = new Mock<IProductService>();
            productServiceMoq.Setup(n => n.GetProduct(It.IsAny<Guid>())).ReturnsAsync(product);
            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new ProductController(productServiceMoq.Object);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };

            /// Act
            var result = await sut.Get(product.Id);

            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var productResult = okResult.Value as Product;
            Assert.IsNotNull(productResult);
            Assert.AreEqual(product.ProductName, productResult.ProductName);
            Assert.AreEqual(product.Cost, productResult.Cost);
            Assert.AreEqual(product.AmountAvailable, productResult.AmountAvailable);
        }

        [TestMethod]
        public async Task AddProduct_Should_SaveSuccesfully()
        {
            var product = new ProductRequest()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7
            };
            var user = new AppUser()
            {
                Id = "1"
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await sut.AddProduct(product, user.Id);

            Assert.AreEqual(1, productRepositoryMockData.Products.Count);
            Assert.AreEqual(product, productRepositoryMockData.Products.FirstOrDefault());
        }

        [TestMethod]
        public async Task UpdateProduct_Should_Throw_Product_NotFoundException_IfNoProductIsFound()
        {
            var product = new Product()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "1",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);

            
            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await Assert.ThrowsExceptionAsync<ProductNotFoundException>(async ()=> await sut.UpdateProduct(product, "1"));
        }

        [TestMethod]
        public async Task UpdateProduct_Should_Throw_UnauthorizedOperationException_IfUserIsNotTheSeller()
        {
            var product = new Product()
            {
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "2",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            productRepositoryMockData.Products.Add(product);
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () => await sut.UpdateProduct(product, "1"));
        }

        [TestMethod]
        public async Task UpdateProduct_Should_UpdateProduct_IfValid()
        {
            var productId = Guid.NewGuid();
            var product = new Product()
            {
                Id = productId,
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "2",
            };
            var updatedProduct = new Product()
            {
                Id = productId,
                ProductName = "TV New",
                Cost = 270,
                AmountAvailable = 3,
                SellerId = "2",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            productRepositoryMockData.Products.Add(product);
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await sut.UpdateProduct(updatedProduct, "2");

            ///Assert
            var productUpdateResult = productRepositoryMockData.Products.FirstOrDefault();
            Assert.IsNotNull(productUpdateResult);
            Assert.AreEqual(updatedProduct.ProductName, productUpdateResult.ProductName);
            Assert.AreEqual(updatedProduct.AmountAvailable, productUpdateResult.AmountAvailable);
            Assert.AreEqual(updatedProduct.Cost, productUpdateResult.Cost);
        }

        [TestMethod]
        public async Task DeleteProduct_Should_DeleteProduct_IfValid()
        {
            var productId = Guid.NewGuid();
            var product = new Product()
            {
                Id = productId,
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "2",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            productRepositoryMockData.Products.Add(product);
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await sut.DeleteProduct(productId, "2");

            ///Assert
            var productUpdateResult = productRepositoryMockData.Products.FirstOrDefault();
            Assert.IsNull(productUpdateResult);
        }

        [TestMethod]
        public async Task DeleteProduct_Should_Throw_Product_NotFoundException_IfNoProductIsFound()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "1",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await Assert.ThrowsExceptionAsync<ProductNotFoundException>(async () => await sut.DeleteProduct(product.Id, "1"));
        }

        [TestMethod]
        public async Task DeleteProduct_Should_Throw_UnauthorizedOperationException_IfUserIsNotTheSeller()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "2",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            productRepositoryMockData.Products.Add(product);
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () => await sut.DeleteProduct(product.Id, "1"));
        }

        [TestMethod]
        public async Task GetProduct_Should_Throw_ProductNotFoundException_IfProductIsNotFound()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                ProductName = "TV",
                Cost = 235,
                AmountAvailable = 7,
                SellerId = "2",
            };
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);


            var sut = new ProductService(unitOfWorkMoq.Object);
            /// Act
            await Assert.ThrowsExceptionAsync<ProductNotFoundException>(async () => await sut.GetProduct(product.Id));
        }
    }
}
