using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Repositories;
using VendingMachine.DAL.Repositories.Interfaces;

namespace VendingMachine.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposed;
        private IDbContextTransaction _dbContextTransaction;
        private readonly ApplicationDbContext _dbContext;
        private IProductRepository _productRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository = _productRepository ?? new ProductRepository(_dbContext); }
        }


        public async Task CreateTransaction()
        {
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.CommitAsync();
            }
        }

        public async Task Rollback()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackAsync();
            }
        }


        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_dbContextTransaction != null)
                    {
                        _dbContextTransaction.Dispose();
                    }
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
