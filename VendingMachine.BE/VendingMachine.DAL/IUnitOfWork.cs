using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Repositories.Interfaces;

namespace VendingMachine.DAL
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        Task Commit();
        Task Rollback();
        Task CreateTransaction();
        Task Save();
    }
}
