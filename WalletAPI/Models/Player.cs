using System;
using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Models
{
    public class Player
    {
        [Required]
        public Guid ID { get; set; }
        [Required]
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}
