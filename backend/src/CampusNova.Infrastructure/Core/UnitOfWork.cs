using System;
using System.Data;
using CampusNova.Domain.Common.Interfaces;

namespace CampusNova.Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction? Transaction => _transaction;

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                return;
            }

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void Dispose()
        {
            DisposeTransaction();
        }

        private void DisposeTransaction()
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
