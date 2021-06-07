using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletAPI.Constants
{
    public class Enums
    {
        public enum TransactionType
        {
            deposit = 0, 
            stake = 1,
            win = 2
        }

        public enum TransactionStatus
        {
            accepted = 0,
            rejected = 1
        }
    }
}
