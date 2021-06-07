using System;
using System.Collections.Generic;
using System.Linq;
using WalletAPI.Contracts;
using WalletAPI.Data;
using WalletAPI.Models;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Repository
{
    public class MockupTransactionRepository : ITransactionRepository
    {
        private readonly SharedMemory _shared;

        public MockupTransactionRepository(SharedMemory shared)
        {
            _shared = shared;
        }

        public TransactionStatus CreateTransaction(Transaction transaction)
        {
            return _shared.CreateTransction(transaction);
        }

        public Transaction GetTransactionById(Guid transactionId)
        {
            return _shared.Transactions.FirstOrDefault(x => x.ID == transactionId);
        }

        public IEnumerable<Transaction> GetTransactions(Guid playerId)
        {
            return _shared.Transactions.Where(x => x.PlayerId == playerId);
        }
    }
}
