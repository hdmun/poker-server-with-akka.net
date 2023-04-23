using Domain.PokerRule.Data;
using Domain.PokerRule.Enums;

namespace Domain.PokerRule.Components
{
    public class TablePlayerComponent
    {
        public int Id { get; private set; }
        public required int Stack { get; set; }
        public required int BetChips { get; set; }
        public required TablePlayerAction LastAction { get; set; }
        public (Card, Card) Hand { get; set; }

        public static TablePlayerComponent Of(int playerId)
        {
            return new TablePlayerComponent
            {
                Id = playerId,
                Stack = 0,
                BetChips = 0,
                LastAction = TablePlayerAction.None,
            };
        }
    }
}
