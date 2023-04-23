using Domain.PokerRule.Data;
using Domain.PokerRule.Enums;
using System;

namespace Domain.PokerRule.Components
{
    public class TableComponent<TPlayer>
    {
        public int Id { get; private set; }
        public required Card[] Boards { get; set; }
        public required int PotChips { get; set; }
        public required int ActionPlayerId { get; set; }
        public required GameState State { get; set; }

        public required Func<Card> GetNextCard { get; set; }
        public required Func<TPlayer[], int> GetNextPosition { get; set; }

        private TableComponent(int id)
        {
            Id = id;
        }

        public static TableComponent<TPlayer> Of(int id)
        {
            var NotNull = new Predicate<TPlayer>(x => x != null);

            return new TableComponent<TPlayer>(id)
            {
                Boards = new Card[] { null, null, null, null, null },
                PotChips = 0,
                ActionPlayerId = 0,
                State = GameState.Waiting,
                GetNextCard = ClosureFunctions.GetFuncArrayShuffler(Card.Cards),
                GetNextPosition = ClosureFunctions.GetFuncArrayNextIndex(0, NotNull),
            };
        }
    }
}
