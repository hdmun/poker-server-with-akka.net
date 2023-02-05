using System;
using System.Linq;

namespace Domain.PokerRule.Data
{
    public class Card
    {
        public static Card Get(Rank rank, Shape shape)
            => Cards[((int)rank)][(int)shape];

        private static Card Of(Rank rank, Shape shape)
            => new Card(rank, shape);

        private static Card[][] Cards = Enum.GetValues<Rank>()
            .Select((rank)
                => Enum.GetValues<Shape>()
                    .Select((shape) => Of(rank, shape))
                    .ToArray())
            .ToArray();

        private readonly Rank _rank;
        private readonly Shape _shape;

        private Card(Rank rank, Shape shape)
        {
            _rank = rank;
            _shape = shape;
        }

        public Rank Rank { get => _rank; }
        public Shape Shape { get => _shape; }

        public int ToMaskIndex() => (int)_rank + (int)_shape * Enum.GetValues(typeof(Rank)).Length;

        public ulong ToBitMask() => 1UL << ToMaskIndex();
    }
}
