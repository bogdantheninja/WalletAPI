using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Models
{
    public class Transaction
    {
        [Required]
        public Guid ID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        [Required]
        public TransactionStatus Status { get; set; }
        [Required]
        public Guid PlayerId { get; set; }
    }
}
