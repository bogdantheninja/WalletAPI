using System;
using System.Collections.Generic;
using WalletAPI.Models;

namespace WalletAPI.Contracts
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetPlayers();
        Player GetPlayerById(Guid playerId);
        void CreatePlayer(Player player);
        void UpdatePlayer(Player player);
    }
}
