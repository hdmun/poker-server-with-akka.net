using Domain.PokerRule.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Domain.Poker.Simulator
{
    public class HandEvaluator
    {
        public static HandEvaluator From() => new HandEvaluator();

        private HandEvaluator() { }

        public HandRank Evaluate(ulong cardsMask)
        {
            const int StriaghtFlushCount = 5;

            // Shape 별로 분리
            uint clubRanks = CardsMask.GetClubs(cardsMask);
            uint heartsRanks = CardsMask.GetHearts(cardsMask);
            uint diamondRanks = CardsMask.GetDiamonds(cardsMask);
            uint spadeRanks = CardsMask.GetSpades(cardsMask);

            uint rankSum = clubRanks | heartsRanks | diamondRanks | spadeRanks;
            int rankCount = CardsMask.BitCount(rankSum);
            if (rankCount >= StriaghtFlushCount)
            {
                // Straight Flush or Flush
                foreach (uint shapeRanks in new uint[]{ clubRanks, heartsRanks, diamondRanks, spadeRanks})
                {
                    if (CardsMask.BitCount(shapeRanks) >= StriaghtFlushCount)
                    {
                        int straightFlushPriority = EvalStraight(shapeRanks);
                        if (straightFlushPriority > 0)
                            return HandRank.StraightFlush;

                        return HandRank.Flush;
                    }
                }

                int straightPriority = EvalStraight(rankSum);
                if (straightPriority > 0)
                    return HandRank.Straight;
            }

            const int EvalCardCount = 7;
            int pairCount = EvalCardCount - rankCount;
            switch (pairCount)
            {
                case 0:
                    // high card + top 5
                    return HandRank.HighCard;
                case 1:
                    // one pair, top 4
                    return HandRank.Pair;
                case 2:
                    uint twoPairMask = rankSum ^ (clubRanks ^ heartsRanks ^ diamondRanks ^ spadeRanks);
                    if (twoPairMask != 0)
                    {
                        // two pair + top 1
                        return HandRank.TwoPair;
                    }
                    else
                    {
                        // three of kind + top 2
                        return HandRank.Trips;
                    }
                default:
                    // four of kind + top 1
                    uint fourOfKindMask = clubRanks & heartsRanks & diamondRanks & spadeRanks;
                    if (fourOfKindMask != 0)
                    {
                        return HandRank.FourOfAKind;
                    }

                    // full house
                    uint twoPairMask2 = rankSum ^ (clubRanks ^ heartsRanks ^ diamondRanks ^ spadeRanks);
                    if (CardsMask.BitCount(twoPairMask2) != pairCount)
                    {
                        return HandRank.FullHouse;
                    }

                    // two pair + top 1
                    return HandRank.TwoPair;
            }
        }

        private int EvalStraight(uint cardsMask)
        {
            if (StraightTable.TryGetValue(cardsMask, out int priority))
                return priority;
            return 0;
        }

        private static readonly IReadOnlyDictionary<uint, int> StraightTable
            = JsonSerializer.Deserialize<Dictionary<uint, int>>(
                File.ReadAllText("./Data/straightTable.json"));
    }
}
