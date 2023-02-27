using VendingMachine.DAL.Entities;

namespace VendingMachine.BLL.Models
{
    /// <summary>
    /// Implement /buy endpoint (accepts productId, amount of products) so users with a “buyer” role can buy products with
    /// the money they’ve deposited. API should return total they’ve spent, products they’ve purchased and their change if 
    /// there’s any (in an array of 5, 10, 20, 50 and 100 cent coins)
    /// </summary>
    public class BuyResponse
    {
        public int TotalSpent { get; set; }

        public Product PurchasedProduct { get; set; }

        public int CountOfPurchasedProducts { get; set; }

        public Change Change { get; set; }
    }
}
