using System;
using System.Collections.Generic;

namespace WalletAPI.Dtos
{
    public class PlayerReadDto
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}
