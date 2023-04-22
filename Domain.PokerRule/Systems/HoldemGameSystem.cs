using Domain.PokerRule.Components;
using Domain.PokerRule.Entities;
using Domain.PokerRule.Extentions;
using System.Collections.Concurrent;

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

        public bool AddPlayer(int tableId, int playerId, int position)
        {
            if (!HoldemTables.TryGetValue(tableId, out var table))
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
            if (!HoldemTables.TryGetValue(tableId, out var table))
            {
                return false;
            }

            var index = table.Players.FindIndex(x => x.Id == playerId);
            if (index < 0)
            {
                return false;
            }

            table.Players[index] = null;
            return true;
        }
    }
}
