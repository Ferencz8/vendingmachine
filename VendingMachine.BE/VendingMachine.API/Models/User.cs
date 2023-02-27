namespace VendingMachine.API.Models
{
    //user model with username, password, deposit and role fields
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int Deposit { get; set; }

        public string Role { get; set; }
    }
}
