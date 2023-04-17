using Domain.PokerRule.Enums;

namespace Domain.PokerRule.Components
{
    public class TablePlayerComponent
    {
        public int Id { get; private set; }
        public int Stack { get; set; }
        public int BetChips { get; set; }
        public TablePlayerAction LastAction { get; set; }

        public static TablePlayerComponent Of(int id)
        {
            return new TablePlayerComponent
            {
                Id = id,
            };
        }
    }
}
