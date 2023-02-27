using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Models;

namespace VendingMachine.BLL
{
    public class AuthenticationSservice : IAuthenticationService
    {
        public AuthenticationSservice()
        {
            
        }
        public Task Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task Register(RegisterUser registerUser)
        {
            throw new NotImplementedException();
        }
    }
}
