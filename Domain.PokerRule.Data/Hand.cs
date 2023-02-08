using System.Linq;

namespace Domain.PokerRule.Data
{
    public class Hand
    {
        public static Hand Of(Card first, Card second)
            => new Hand(first, second);

        private readonly Card[] _cards;

        private Hand(Card first, Card second)
        {
            _cards = new Card[] { first, second };
        }

        public ulong ToBitMask()
            => _cards.Aggregate(0UL, (mask, card) => mask |= card.ToBitMask());
    }
}
