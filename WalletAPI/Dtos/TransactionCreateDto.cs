using System;
using System.ComponentModel.DataAnnotations;
using WalletAPI.Validations;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Dtos
{
    public class TransactionCreateDto
    {
        [Required]
        [GUIDNotEmpty]
        public Guid ID { get; set; }
        [Range(0, 9999999999999999.99, ErrorMessage = "Min Deposit Amount must be a number between 0 and 9999999999999999.99")]
        [RegularExpression(@"^([0-9]|[1-9][0-9]+)(\.[0-9][0-9]?)?$", ErrorMessage = "Invalid Amount, only two decimals allowed")]
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        //public TransactionStatus Status { get; set; }
        [Required]
        public Guid? PlayerId { get; set; }
    }
}
