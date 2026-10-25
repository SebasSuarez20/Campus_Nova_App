using System;

namespace CampusNova.Domain.Exceptions
{
    public class TransactionRollbackException : Exception
    {
        public TransactionRollbackException() 
            : base("La transacción ha sido cancelada y revertida (Rollback).")
        {
        }

        public TransactionRollbackException(string message) 
            : base(message)
        {
        }

        public TransactionRollbackException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
