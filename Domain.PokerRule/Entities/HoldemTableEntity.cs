using Domain.PokerRule.Components;
using Domain.PokerRule.Enums;

namespace Domain.PokerRule.Entities
{
    public class HoldemTableEntity
    {
        public readonly TableComponent Table;
        public readonly TablePlayerComponent[] Players;

        public GameState State { get => Table.State; }

        private HoldemTableEntity(int tableId, int sitCount)
        {
            Table = TableComponent.Of(tableId);
            Players = new TablePlayerComponent[sitCount];
        }

        public static HoldemTableEntity Of(int tableId, int sitCount)
        {
            return new HoldemTableEntity(tableId, sitCount);
        }
    }
}
