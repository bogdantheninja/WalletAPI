using System;
using System.Collections.Generic;
using System.Linq;
using WalletAPI.Constants;
using WalletAPI.Models;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Data
{
    public class SharedMemory
    {
        private static readonly object _syncRoot = new object();

        private readonly List<Player> _players = new List<Player>()
            {
                    new Player { ID = Guid.Parse("122bdd09-f8a3-4619-ac75-82939878d23a"), UserName="Jordan", Balance= 70 },
                    new Player { ID = Guid.Parse("a9ab7488-f1f0-4532-a45b-101520df919c"), UserName="Iverson", Balance= 115 },
                    new Player { ID = Guid.Parse("e4af2666-3c85-427b-b1c7-6785a80f7c29"), UserName="Bryant", Balance= 221.87M }
            };

        private readonly List<Transaction> _transactions= new List<Transaction>()
            {
                new Transaction { ID = Guid.Parse("48f680b1-bd40-4457-8372-a1a539589291"), Amount = 30.5M, Type = Enums.TransactionType.deposit, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("122bdd09-f8a3-4619-ac75-82939878d23a") },
                new Transaction { ID = Guid.Parse("aa23836e-1836-40a0-9486-59f3ae761fe8"), Amount = 10.5M, Type = Enums.TransactionType.stake, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("122bdd09-f8a3-4619-ac75-82939878d23a") },
                new Transaction { ID = Guid.Parse("a87ac6bc-526a-41c8-8d5a-f8b13e4e514e"), Amount = 50M, Type = Enums.TransactionType.win, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("122bdd09-f8a3-4619-ac75-82939878d23a") },
                new Transaction { ID = Guid.Parse("8efcc455-c97c-4575-a046-158bc3dfb24b"), Amount = 25M,Type = Enums.TransactionType.deposit, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("a9ab7488-f1f0-4532-a45b-101520df919c") },
                new Transaction { ID = Guid.Parse("56fe5b11-7fd5-48cc-8b7b-709c1c886568"), Amount = 15M, Type = Enums.TransactionType.stake, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("a9ab7488-f1f0-4532-a45b-101520df919c") },
                new Transaction { ID = Guid.Parse("5ea9d65d-a385-414c-94b7-05324ca1b9bb"), Amount = 105M, Type = Enums.TransactionType.win, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("a9ab7488-f1f0-4532-a45b-101520df919c") },
                new Transaction { ID = Guid.Parse("185e9bf6-c460-42cc-8440-243fca4ff1d8"), Amount = 34.12M, Type = Enums.TransactionType.deposit, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("e4af2666-3c85-427b-b1c7-6785a80f7c29") },
                new Transaction { ID = Guid.Parse("fb7193b4-0c74-4298-8f87-83b94f1731c3"), Amount = 13.23M, Type = Enums.TransactionType.stake, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("e4af2666-3c85-427b-b1c7-6785a80f7c29") },
                new Transaction { ID = Guid.Parse("f0aae91f-b596-4f1f-bcb4-db54bbd6891d"), Amount = 200.98M, Type = Enums.TransactionType.win, Status = Enums.TransactionStatus.accepted, PlayerId = Guid.Parse("e4af2666-3c85-427b-b1c7-6785a80f7c29") },
            };


        public List<Player> Players => _players;
        public List<Transaction> Transactions => _transactions;

        public void UpdatePlayer(Player player)
        {
            if (_players.FirstOrDefault(x => x.UserName == player.UserName) != null)
            {
                throw new Exception("Player with username " + player.UserName + " already exists");
            }
            else
            {
                lock (_syncRoot)
                {
                    var playerEntity = _players.FirstOrDefault(x => x.ID == player.ID);
                    playerEntity.UserName = player.UserName;
                }
            }
        }

        public void CreatePlayer(Player player)
        {
            if (_players.Exists(p => p.ID == player.ID || p.UserName == player.UserName))
            {
                throw new Exception("Player with this username or id already exists");
            }
            else
            {
                lock (_syncRoot)
                {
                    _players.Add(player);
                }
            }
        }

        public TransactionStatus CreateTransction(Transaction transaction)
        {
            try
            {
                lock (_syncRoot)
                {
                    var transactionEntity = _transactions.FirstOrDefault(x => x.ID == transaction.ID);// #TODO SAME SERVER STATE MEANS EQUAL BY ID OR EQUAL OBJECTS?
                    if (transactionEntity != null)
                    {
                        return transactionEntity.Status;
                    }

                    var playerEntity = _players.FirstOrDefault(x => x.ID == transaction.PlayerId);
                    if (playerEntity != null)
                    {
                        switch (transaction.Type)
                        {
                            case TransactionType.deposit:
                            case TransactionType.win:
                                playerEntity.Balance += transaction.Amount;
                                transaction.Status = TransactionStatus.accepted;
                                _transactions.Add(transaction);
                                break;
                            case TransactionType.stake:
                                if (playerEntity.Balance - transaction.Amount >= 0)
                                {
                                    playerEntity.Balance -= transaction.Amount;
                                    transaction.Status = TransactionStatus.accepted;
                                    _transactions.Add(transaction);
                                }
                                else
                                {
                                    transaction.Status = TransactionStatus.rejected;
                                    _transactions.Add(transaction);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                return transaction.Status;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Transaction Failed: {0}", ex.Message));
            }
        }
    }
}
