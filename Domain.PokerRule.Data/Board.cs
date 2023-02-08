using System.Linq;

namespace Domain.PokerRule.Data
{
    public class Board
    {
        public static Board From() => new();
        public static readonly int MaxCardCount = 5;

        public readonly Card[] _cards;

        public Board()
        {
            _cards = new Card[MaxCardCount];
        }

        public int CardCount()
            => _cards.Where(hand => hand != null).Count();

        public void OnFlop(Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                _cards[i] = cards[i];
            }
        }

        public void OnTrun(Card trunCard)
        {
            _cards[3] = trunCard;
        }

        public void OnRiver(Card riverCard)
        {
            _cards[4] = riverCard;
        }

        public ulong ToBitMask()
            => _cards.Where((card) => card != null)
                    .Aggregate(0UL, (bitMask, card) => bitMask |= card.ToBitMask());
    }
}
