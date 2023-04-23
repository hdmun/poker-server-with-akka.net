using Domain.PokerRule.Components;
using Domain.PokerRule.Data;
using Domain.PokerRule.Entities;
using Domain.PokerRule.Extentions;
using System.Collections.Concurrent;
using System.Linq;

namespace Domain.PokerRule.Systems
{
    public class HoldemGameSystem
    {
        public readonly ConcurrentDictionary<int, HoldemTableEntity> HoldemTables;

        public HoldemGameSystem()
        {
            HoldemTables = new();
        }

        public void Initialize()
        {
            const int tableCount = 10;
            for (int i = 1; i <= tableCount; i++)
            {
                HoldemTables.TryAdd(i, HoldemTableEntity.Of(i, 6));
            }
        }

        private HoldemTableEntity GetTableEntity(int tableId)
        {
            return HoldemTables.TryGetValue(tableId, out var table) ? table : default;
        }

        public bool AddPlayer(int tableId, int playerId, int position)
        {
            var table = GetTableEntity(tableId);
            if (table == null)
            {
                return false;
            }

            if (!table.Players.IsInBounds(position))
            {
                return false;
            }

            if (table.Players[position] != null)
            {
                return false;
            }

            table.Players[position] = TablePlayerComponent.Of(playerId);
            return true;
        }

        public bool RemovePlayer(int tableId, long playerId)
        {
            var table = GetTableEntity(tableId);
            if (table == null)
            {
                return false;
            }

            var index = table.Players.FindIndex(x => x?.Id == playerId);
            if (index < 0)
            {
                return false;
            }

            table.Players[index] = null;
            return true;
        }

        public void Start(int tableId)
        {
            var tableEntity = GetTableEntity(tableId);
            if (tableEntity == null)
            {
                return;
            }

            if (tableEntity.PlayerCount < 2)
            {
                return;
            }

            var tableComponent = tableEntity.Table;

            // card shuflle
            tableComponent.GetNextCard = ClosureFunctions.GetFuncArrayShuffler(Card.Cards);

            // change dealer btn
            int position = tableComponent.GetNextPosition(tableEntity.Players);

            // distribute card
            var activePlayers = tableEntity.Players
                .Where(x => x != null)
                .ForEachFully(position);
            foreach (var player in activePlayers)
            {
                player.Hand = (tableComponent.GetNextCard(), tableComponent.GetNextCard());
            }

            tableComponent.ActionPlayerId = activePlayers.First().Id;
        }
    }
}
