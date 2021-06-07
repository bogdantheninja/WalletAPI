using System;
using System.Collections.Generic;
using WalletAPI.Models;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Contracts
{
    public interface ITransactionRepository
    {
        TransactionStatus CreateTransaction(Transaction transaction);
        Transaction GetTransactionById(Guid transactionId);
        IEnumerable<Transaction> GetTransactions(Guid playerId);
    }
}
