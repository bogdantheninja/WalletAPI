using System;
using System.ComponentModel.DataAnnotations;
using WalletAPI.Validations;

namespace WalletAPI.Dtos
{
    public class PlayerCreateDto
    {
        [Required]
        [GUIDNotEmpty]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(12)]
        public string UserName { get; set; }
    }
}
