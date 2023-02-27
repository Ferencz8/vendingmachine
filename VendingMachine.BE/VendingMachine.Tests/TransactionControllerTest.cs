using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VendingMachine.API.Controllers;
using VendingMachine.API.Models;
using VendingMachine.BLL;
using VendingMachine.BLL.Exceptions;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Models;
using VendingMachine.DAL;
using VendingMachine.DAL.Entities;
using VendingMachine.Tests.MockData;

namespace VendingMachine.Tests
{
    /// <summary>
    /// Note: I tested together both the flow of the endpoint, since this is what was required and the flow of the TransactionService, since this is where the actual business logic was done
    /// </summary>
    [TestClass]
    public class TransactionControllerTest
    {
        private ITransactionService _transactionService;
        private ProductRepositoryFake _productRepositoryFake;
        private Mock<IUnitOfWork> _unitOfWorkMoq;
        private Mock<IUserStore<AppUser>> _userStoreMoq;
        private Mock<UserManager<AppUser>> _userManagerMoq;
        private Mock<HttpContext> _httpCtxMoq;
        
        [TestInitialize()]
        public void Setup()
        {
            // This method will be called before each MSTest test method
            
            _productRepositoryFake = new ProductRepositoryFake();
            _unitOfWorkMoq = new Mock<IUnitOfWork>();            
            _userStoreMoq = new Mock<IUserStore<AppUser>>();
            _userManagerMoq = new Mock<UserManager<AppUser>>(_userStoreMoq.Object, null, null, null, null, null, null, null, null);            
            _httpCtxMoq = new Mock<HttpContext>();
        }

        private TransactionController SetupControllerWithDependencies(AppUser user)
        {
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryMockData = new ProductRepositoryFake();
            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryMockData);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _transactionService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);

            var sut = new TransactionController(_transactionService);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = httpCtxMock.Object };

            return sut;
        }

        [TestMethod]
        public async Task ShouldThrowError_IfDeposit_IsNotValid()
        {
            var user = new AppUser()
            {
                Id = "1"
            };
            var sut = SetupControllerWithDependencies(user);

            await Assert.ThrowsExceptionAsync<Exception>(
                async () => await sut.Deposit(135),
                $"Depositing amount of 135 is not permitted. Only values 5, 10, 20, 50, 100 are allowed.");
        }

        //TODO: remove or refactor
        [TestMethod]
        public async Task ShouldThrowError_IfDeposit_IsNotValid2()
        {
            var user = new AppUser()
            {
                Id = "1"
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);

            _httpCtxMoq.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            _unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(_productRepositoryFake);
            _userManagerMoq.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactionService = new TransactionService(_unitOfWorkMoq.Object, _userManagerMoq.Object);

            var sut = new TransactionController(transactionService);
            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            { HttpContext = _httpCtxMoq.Object };


            await Assert.ThrowsExceptionAsync<Exception>(
                async () => await sut.Deposit(135),
                $"Depositing amount of 135 is not permitted. Only values 5, 10, 20, 50, 100 are allowed.");
        }

        [TestMethod]
        public async Task ShouldReturnSuccessfullResult_IfDeposit_ValidInputIsSent()
        {
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 10
            };
            var sut = SetupControllerWithDependencies(user);

            /// Act
            var result = await sut.Deposit(5);

            /// Assert
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(15, user.Deposit);
        }

        [TestMethod]
        public async Task Should_AmountOfProducts_And_MMoneyDeposit_BeSufficient_Then_BuyIsIsSuccessfull()
        {
            var expectedChange = new Change()
            {
                FiftyCents = 1,
                TwentyCents = 1,
                TenCents = 1
            };
            var productId = Guid.NewGuid();
            var buyRequest = new BuyRequest() { ProductId = productId, AmountOfProducts = 3 };
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 95
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryFake = new ProductRepositoryFake();
            productRepositoryFake.Products.Add(new DAL.Entities.Product()
            {
                Id = productId,
                SellerId = "2",
                AmountAvailable = 10,
                Cost = 5,
                ProductName = "TV"
            });

            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryFake);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactioService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new TransactionController(transactioService);
            sut.ControllerContext = new ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Buy(buyRequest);

            /// Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(80, user.Deposit);


            var buyResponse = okResult.Value as BuyResponse;
            Assert.IsNotNull(buyResponse);
            Assert.AreEqual(15, buyResponse.TotalSpent);
            Assert.AreEqual(3, buyResponse.CountOfPurchasedProducts);
            Assert.IsNotNull(buyResponse.PurchasedProduct);
            Assert.AreEqual(productId, buyResponse.PurchasedProduct.Id);
            Assert.IsNotNull(buyResponse.Change);
            Assert.AreEqual(expectedChange.FiveCents, buyResponse.Change.FiveCents);
            Assert.AreEqual(expectedChange.TenCents, buyResponse.Change.TenCents);
            Assert.AreEqual(expectedChange.TwentyCents, buyResponse.Change.TwentyCents);
            Assert.AreEqual(expectedChange.FiftyCents, buyResponse.Change.FiftyCents);
            Assert.AreEqual(expectedChange.HundredCents, buyResponse.Change.HundredCents);
        }

        [TestMethod]
        public async Task Should_AmountOfProducts_BeLower_Than_Expected_Then_AllProductsAreSold()
        {
            var expectedChange = new Change()
            {
                FiftyCents = 0,
                TwentyCents = 2,
                TenCents = 0,
                HundredCents = 1,
                FiveCents = 1
            };
            var productId = Guid.NewGuid();
            var buyRequest = new BuyRequest() { ProductId = productId, AmountOfProducts = 5 };
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 190
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryFake = new ProductRepositoryFake();
            productRepositoryFake.Products.Add(new DAL.Entities.Product()
            {
                Id = productId,
                SellerId = "2",
                AmountAvailable = 3,
                Cost = 15,
                ProductName = "XBox"
            });

            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryFake);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactioService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new TransactionController(transactioService);
            sut.ControllerContext = new ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Buy(buyRequest);

            /// Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(145, user.Deposit);


            var buyResponse = okResult.Value as BuyResponse;
            Assert.IsNotNull(buyResponse);
            Assert.AreEqual(45, buyResponse.TotalSpent);
            Assert.AreEqual(3, buyResponse.CountOfPurchasedProducts);
            Assert.IsNotNull(buyResponse.PurchasedProduct);
            Assert.AreEqual(productId, buyResponse.PurchasedProduct.Id);
            Assert.IsNotNull(buyResponse.Change);
            Assert.AreEqual(expectedChange.FiveCents, buyResponse.Change.FiveCents);
            Assert.AreEqual(expectedChange.TenCents, buyResponse.Change.TenCents);
            Assert.AreEqual(expectedChange.TwentyCents, buyResponse.Change.TwentyCents);
            Assert.AreEqual(expectedChange.FiftyCents, buyResponse.Change.FiftyCents);
            Assert.AreEqual(expectedChange.HundredCents, buyResponse.Change.HundredCents);
        }

        [TestMethod]
        public async Task Should_MoneyDeposit_BeLower_Than_TotalCost_Then_OnlyProductsWhichAreAffordableAreSold()
        {
            var expectedChange = new Change()
            {
                FiftyCents = 0,
                TwentyCents = 1,
                TenCents = 0,
                HundredCents = 0,
                FiveCents = 1
            };
            var productId = Guid.NewGuid();
            var buyRequest = new BuyRequest() { ProductId = productId, AmountOfProducts = 4 };
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 95
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryFake = new ProductRepositoryFake();
            productRepositoryFake.Products.Add(new DAL.Entities.Product()
            {
                Id = productId,
                SellerId = "2",
                AmountAvailable = 15,
                Cost = 35,
                ProductName = "Headset"
            });

            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryFake);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactioService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new TransactionController(transactioService);
            sut.ControllerContext = new ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Buy(buyRequest);

            /// Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(25, user.Deposit);


            var buyResponse = okResult.Value as BuyResponse;
            Assert.IsNotNull(buyResponse);
            Assert.AreEqual(70, buyResponse.TotalSpent);
            Assert.AreEqual(2, buyResponse.CountOfPurchasedProducts);
            Assert.IsNotNull(buyResponse.PurchasedProduct);
            Assert.AreEqual(productId, buyResponse.PurchasedProduct.Id);
            Assert.IsNotNull(buyResponse.Change);
            Assert.AreEqual(expectedChange.FiveCents, buyResponse.Change.FiveCents);
            Assert.AreEqual(expectedChange.TenCents, buyResponse.Change.TenCents);
            Assert.AreEqual(expectedChange.TwentyCents, buyResponse.Change.TwentyCents);
            Assert.AreEqual(expectedChange.FiftyCents, buyResponse.Change.FiftyCents);
            Assert.AreEqual(expectedChange.HundredCents, buyResponse.Change.HundredCents);
        }

        [TestMethod]
        public async Task Should_Throw_InsuficientFundsException_IfMoneyDeposit_IsNotEnoughToBuy()
        {
            var productId = Guid.NewGuid();
            var buyRequest = new BuyRequest() { ProductId = productId, AmountOfProducts = 2 };
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 95
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryFake = new ProductRepositoryFake();
            productRepositoryFake.Products.Add(new DAL.Entities.Product()
            {
                Id = productId,
                SellerId = "2",
                AmountAvailable = 15,
                Cost = 105,
                ProductName = "Car"
            });

            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryFake);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactioService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new TransactionController(transactioService);
            sut.ControllerContext = new ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            await Assert.ThrowsExceptionAsync<InsuficientFundsException>(async () => await sut.Buy(buyRequest));
        }

        [TestMethod]
        public async Task Should_MoneyDeposit_BeLower_Than_TotalCost_AndStockIsNotEnough_OnlyProductsWhichAreAffordableAreSold()
        {
            var expectedChange = new Change()
            {
                FiftyCents = 0,
                TwentyCents = 0,
                TenCents = 1,
                HundredCents = 0,
                FiveCents = 0
            };
            var productId = Guid.NewGuid();
            var buyRequest = new BuyRequest() { ProductId = productId, AmountOfProducts = 5 };
            var user = new AppUser()
            {
                Id = "1",
                Deposit = 50
            };
            var claim = new Claim(ClaimTypes.NameIdentifier, user.Id);
            var productRepositoryFake = new ProductRepositoryFake();
            productRepositoryFake.Products.Add(new DAL.Entities.Product()
            {
                Id = productId,
                SellerId = "2",
                AmountAvailable = 4,
                Cost = 20,
                ProductName = "Cable"
            });

            var unitOfWorkMoq = new Mock<IUnitOfWork>();
            unitOfWorkMoq.Setup(uow => uow.ProductRepository).Returns(productRepositoryFake);
            var store = new Mock<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var transactioService = new TransactionService(unitOfWorkMoq.Object, userManagerMock.Object);

            var httpCtxMock = new Mock<HttpContext>();
            httpCtxMock.Setup(h => h.User.FindFirst(It.IsAny<string>())).Returns(claim);
            var sut = new TransactionController(transactioService);
            sut.ControllerContext = new ControllerContext()
            { HttpContext = httpCtxMock.Object };
            /// Act
            var result = await sut.Buy(buyRequest);

            /// Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(10, user.Deposit);


            var buyResponse = okResult.Value as BuyResponse;
            Assert.IsNotNull(buyResponse);
            Assert.AreEqual(40, buyResponse.TotalSpent);
            Assert.AreEqual(2, buyResponse.CountOfPurchasedProducts);
            Assert.IsNotNull(buyResponse.PurchasedProduct);
            Assert.AreEqual(productId, buyResponse.PurchasedProduct.Id);
            Assert.IsNotNull(buyResponse.Change);
            Assert.AreEqual(expectedChange.FiveCents, buyResponse.Change.FiveCents);
            Assert.AreEqual(expectedChange.TenCents, buyResponse.Change.TenCents);
            Assert.AreEqual(expectedChange.TwentyCents, buyResponse.Change.TwentyCents);
            Assert.AreEqual(expectedChange.FiftyCents, buyResponse.Change.FiftyCents);
            Assert.AreEqual(expectedChange.HundredCents, buyResponse.Change.HundredCents);
        }
    }
}