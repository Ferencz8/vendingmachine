namespace VendingMachine.API.Models
{
    //amountAvailable, cost, productName and sellerId
    public class ProductRequest
    {
        public int AmountAvailable { get; set; }

        public int Cost { get; set; }

        public string ProductName { get; set; }
    }
}
