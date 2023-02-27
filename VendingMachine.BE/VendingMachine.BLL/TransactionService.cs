using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BLL.Exceptions;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Models;
using VendingMachine.DAL;
using VendingMachine.DAL.Entities;

namespace VendingMachine.BLL
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public TransactionService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<BuyResponse> Buy(Guid productId, int amountOfProducts, string userId)
        {
            var response = new BuyResponse();
            var user = await _userManager.FindByIdAsync(userId);
            var productEntity = await _unitOfWork.ProductRepository.GetById(productId);
            int amountOfProductsUserCanAfford = user.Deposit / productEntity.Cost;
            if (amountOfProductsUserCanAfford <= 0)
            {
                throw new InsuficientFundsException(productId);
            }

            if (productEntity.AmountAvailable >= amountOfProducts)
            {
                int totalCost = productEntity.Cost * amountOfProducts;
                if (user.Deposit >= totalCost)
                {
                    productEntity.AmountAvailable -= amountOfProducts;
                    user.Deposit -= totalCost;
                    response.TotalSpent = totalCost;
                    response.CountOfPurchasedProducts = amountOfProducts;
                }
                else
                {
                    int totalAffordableCost = productEntity.Cost * amountOfProductsUserCanAfford;
                    productEntity.AmountAvailable -= amountOfProductsUserCanAfford;
                    user.Deposit -= totalAffordableCost;
                    response.TotalSpent = totalAffordableCost;
                    response.CountOfPurchasedProducts = amountOfProductsUserCanAfford;
                }
            }
            else
            {
                int totalCostOfAvailableProducts = productEntity.Cost * productEntity.AmountAvailable;
                if (user.Deposit >= totalCostOfAvailableProducts)
                {
                    user.Deposit -= totalCostOfAvailableProducts;
                    response.TotalSpent = totalCostOfAvailableProducts;
                    response.CountOfPurchasedProducts = productEntity.AmountAvailable;
                    productEntity.AmountAvailable = 0;
                }
                else
                {
                    int totalAffordableCost = productEntity.Cost * amountOfProductsUserCanAfford;
                    productEntity.AmountAvailable -= amountOfProductsUserCanAfford;
                    user.Deposit -= totalAffordableCost;
                    response.TotalSpent = totalAffordableCost;
                    response.CountOfPurchasedProducts = amountOfProductsUserCanAfford;
                }
            }

            response.Change = CalculateChange(user.Deposit);
            response.PurchasedProduct = productEntity;
            await _unitOfWork.Save();
            return response;
        }

        private Change CalculateChange(int remainedDeposit)
        {
            var change = new Change();
            var countOfOneHundredCents = remainedDeposit / 100;
            if (countOfOneHundredCents > 0)
            {
                remainedDeposit -= 100 * countOfOneHundredCents;
                change.HundredCents = countOfOneHundredCents;
            }
            var countOfFiftyCents = remainedDeposit / 50;
            if (countOfFiftyCents > 0)
            {
                remainedDeposit -= 50 * countOfFiftyCents;
                change.FiftyCents = countOfFiftyCents;
            }
            var countOfTwentyCents = remainedDeposit / 20;
            if (countOfTwentyCents > 0)
            {
                remainedDeposit -= 20 * countOfTwentyCents;
                change.TwentyCents = countOfTwentyCents;
            }
            var countOfTenCents = remainedDeposit / 10;
            if (countOfTenCents > 0)
            {
                remainedDeposit -= 10 * countOfTenCents;
                change.TenCents = countOfTenCents;
            }
            var countOfFiveCents = remainedDeposit / 5;
            if (countOfFiveCents > 0)
            {
                remainedDeposit -= 5 * countOfFiveCents;
                change.FiveCents = countOfFiveCents;
            }
            return change;
        }

        public async Task Deposit(int amount, string userId)
        {
            if (!(amount == 5 || amount == 10 || amount == 20 || amount == 50 || amount == 100))
            {
                throw new Exception($"Depositing amount of {amount} is not permitted. Only values 5, 10, 20, 50, 100 are allowed.");
            }

            var userEntity = await _userManager.FindByIdAsync(userId);

            userEntity.Deposit += amount;
            await _userManager.UpdateAsync(userEntity);
        }

        public async Task Reset(string userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);
            userEntity.Deposit = 0;
            await _userManager.UpdateAsync(userEntity);
        }
    }
}
