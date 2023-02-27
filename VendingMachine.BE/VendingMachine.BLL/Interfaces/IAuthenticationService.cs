using VendingMachine.BLL.Models;

namespace VendingMachine.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task Register(RegisterUser registerUser);

        Task Login(string username, string password);
    }
}
