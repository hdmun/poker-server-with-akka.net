using Domain.PokerRule.Components;
using Domain.PokerRule.Enums;
using System.Linq;

namespace Domain.PokerRule.Entities
{
    public class HoldemTableEntity
    {
        public readonly TableComponent<TablePlayerComponent> Table;
        public readonly TablePlayerComponent[] Players;

        public GameState State { get => Table.State; }
        public int PlayerCount { get => Players.Where(x => x !=null).Count(); }

        private HoldemTableEntity(int tableId, int sitCount)
        {
            Table = TableComponent<TablePlayerComponent>.Of(tableId);
            Players = new TablePlayerComponent[sitCount];
        }

        public static HoldemTableEntity Of(int tableId, int sitCount)
        {
            return new HoldemTableEntity(tableId, sitCount);
        }
    }
}
