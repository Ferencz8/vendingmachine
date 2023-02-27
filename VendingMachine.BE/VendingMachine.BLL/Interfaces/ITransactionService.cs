using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BLL.Models;
using VendingMachine.DAL.Migrations;

namespace VendingMachine.BLL.Interfaces
{
    public interface ITransactionService
    {
        Task Deposit(int amount, string userId);

        Task<BuyResponse> Buy(Guid productId, int amountOfProducts, string userId);

        Task Reset(string userId);
    }
}
