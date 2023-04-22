using Domain.PokerRule.Data;
using Domain.PokerRule.Enums;

namespace Domain.PokerRule.Components
{
    public class TableComponent
    {
        public int Id { get; private set; }
        public required Card[] Boards { get; set; }
        public required int PotChips { get; set; }
        public required int ActionPlayerId { get; set; }
        public required GameState State { get; set; }

        private TableComponent(int id)
        {
            Id = id;
        }

        public static TableComponent Of(int id)
        {
            return new TableComponent(id)
            {
                Boards = new Card[] { null, null, null, null, null },
                PotChips = 0,
                ActionPlayerId = 0,
                State = GameState.Waiting,
            };
        }
    }
}
