using System;
using System.Linq;

namespace Domain.PokerRule.Data
{
    public class Card
    {
        public static Card Get(Rank rank, Shape shape)
            => Cards[((int)rank) + ((int)shape * RankCount)];

        private static Card Of(Rank rank, Shape shape)
            => new Card(rank, shape);

        public readonly static int RankCount = Enum.GetValues<Rank>().Length;

        public readonly static Card[] Cards = Enum.GetValues<Shape>()
            .SelectMany((shape)
                => Enum.GetValues<Rank>()
                    .Select((rank) => Of(rank, shape)))
            .ToArray();

        public static int DeckCount = Cards.Length;

        private readonly Rank _rank;
        private readonly Shape _shape;

        private Card(Rank rank, Shape shape)
        {
            _rank = rank;
            _shape = shape;
        }

        public Rank Rank { get => _rank; }
        public Shape Shape { get => _shape; }

        public int ToMaskIndex() => (int)_rank + (int)_shape * RankCount;

        public ulong ToBitMask() => 1UL << ToMaskIndex();
    }
}
