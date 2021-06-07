using System;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Dtos
{
    public class TransactionReadDto
    {
        public Guid ID { get; set; }
        public decimal Amount { get; set; }
        //public TransactionType Type { get; set; }
        //public TransactionStatus Status { get; set; }
        //public Guid PlayerId { get; set; }
    }
}
