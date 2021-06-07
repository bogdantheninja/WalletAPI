using System;
using System.Collections.Generic;
using System.Linq;
using WalletAPI.Contracts;
using WalletAPI.Data;
using WalletAPI.Models;

namespace WalletAPI.Repository
{
    public class MockupPlayersRepository : IPlayerRepository
    {
        private readonly SharedMemory _shared;

        public MockupPlayersRepository(SharedMemory shared)
        {
            _shared = shared;
        }

        public Player GetPlayerById(Guid id)
        {
            return _shared.Players.FirstOrDefault(p => p.ID == id);
        }
        public IEnumerable<Player> GetPlayers()
        {
            return _shared.Players.ToList();
        }

        public void CreatePlayer(Player player)
        {
            _shared.CreatePlayer(player);
        }

        public void UpdatePlayer(Player player)
        {
            _shared.UpdatePlayer(player);
        }
    }
}
