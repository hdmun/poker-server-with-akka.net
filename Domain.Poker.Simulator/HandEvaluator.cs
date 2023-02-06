using Domain.Poker.Simulator.Model;
using Domain.PokerRule.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Domain.Poker.Simulator
{
    public class HandEvaluator
    {
        public static HandEvaluator From() => new HandEvaluator();

        private HandEvaluator() { }

        public HandValue Evaluate(ulong cardsMask)
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
                        uint straightFlushPriority = EvalStraight(shapeRanks);
                        if (straightFlushPriority > 0)
                            return HandValue.Of(HandRank.StraightFlush, straightFlushPriority, 0);

                        uint kickers = shapeRanks;
                        if (rankCount == StriaghtFlushCount)
                            kickers = TopFiveCardTable[shapeRanks];
                        return HandValue.Of(HandRank.Flush, 0, kickers);
                    }
                }

                uint straightPriority = EvalStraight(rankSum);
                if (straightPriority > 0)
                    return HandValue.Of(HandRank.Straight, straightPriority, 0);
            }

            uint pairMask = rankSum ^ (clubRanks ^ heartsRanks ^ diamondRanks ^ spadeRanks);
            uint tripsMask = ((clubRanks & heartsRanks) | (diamondRanks & spadeRanks)) & ((clubRanks & diamondRanks) | (heartsRanks & spadeRanks));

            const int EvalCardCount = 7;
            int pairCount = EvalCardCount - rankCount;
            switch (pairCount)
            {
                case 0:
                    // high card, top 5
                    uint highCardKickers = rankSum;
                    if (rankCount > 5)
                        highCardKickers = TopFiveCardTable[rankSum];

                    return HandValue.Of(HandRank.HighCard, 0, highCardKickers);
                case 1:
                    {
                        // one pair, top 3 kickers
                        uint kickers = rankSum ^ pairMask;
                        uint pairKickers = TopCardTable[kickers];
                        pairKickers |= TopCardTable[kickers ^ pairKickers];
                        pairKickers |= TopCardTable[kickers ^ pairKickers];
                        return HandValue.Of(HandRank.Pair, pairMask, pairKickers);
                    }
                case 2:
                    if (pairMask != 0)
                    {
                        // two pair, top 1
                        uint twoPairkickers = rankSum ^ pairMask;
                        if (CardsMask.BitCount(twoPairkickers) > 1)
                            twoPairkickers = TopCardTable[twoPairkickers];

                        return HandValue.Of(HandRank.TwoPair, pairMask, twoPairkickers);
                    }
                    else
                    {
                        // three of kind, top 2 kickers
                        uint kickers = rankSum ^ tripsMask;
                        uint tripsKickers = TopCardTable[kickers];
                        tripsKickers |= TopCardTable[kickers ^ tripsKickers];
                        return HandValue.Of(HandRank.Trips, tripsMask, tripsKickers);
                    }
                default:
                    // four of kind
                    uint fourOfKindMask = clubRanks & heartsRanks & diamondRanks & spadeRanks;
                    if (fourOfKindMask != 0)
                    {
                        // kicker top 1
                        uint fourOfAKindKickers = rankSum ^ fourOfKindMask;
                        if (CardsMask.BitCount(fourOfAKindKickers) > 1)
                            fourOfAKindKickers = TopCardTable[fourOfAKindKickers];

                        return HandValue.Of(HandRank.FourOfAKind, fourOfKindMask, fourOfAKindKickers);
                    }

                    // full house
                    if (CardsMask.BitCount(pairMask) != pairCount)
                    {
                        uint title = tripsMask;
                        if (CardsMask.BitCount(title) > 1)
                            title = TopCardTable[title];

                        uint secondPair = (pairMask | tripsMask) ^ title;
                        if (CardsMask.BitCount(secondPair) > 1)
                            secondPair = TopCardTable[secondPair];

                        return HandValue.Of(HandRank.FullHouse, title, secondPair);
                    }

                    {
                        // two pair, top 1
                        uint topTwoPairs = pairMask;
                        if (CardsMask.BitCount(topTwoPairs) > 2)
                        {
                            topTwoPairs = TopCardTable[pairMask];
                            topTwoPairs |= TopCardTable[pairMask ^ topTwoPairs];
                        }

                        uint twoPairkickers = rankSum ^ topTwoPairs;
                        if (CardsMask.BitCount(twoPairkickers) > 1)
                            twoPairkickers = TopCardTable[twoPairkickers];

                        return HandValue.Of(HandRank.TwoPair, topTwoPairs, twoPairkickers);
                    }
            }
        }

        private uint EvalStraight(uint cardsMask)
        {
            if (StraightTable.TryGetValue(cardsMask, out uint priority))
                return priority;
            return 0;
        }

        private static readonly IReadOnlyDictionary<uint, uint> StraightTable
            = JsonSerializer.Deserialize<Dictionary<uint, uint>>(
                File.ReadAllText("./Data/straightTable.json"));

        private static readonly IReadOnlyDictionary<uint, uint> TopFiveCardTable
            = JsonSerializer.Deserialize<Dictionary<uint, uint>>(
                File.ReadAllText("./Data/topFiveCardTable.json"));

        private static readonly IReadOnlyDictionary<uint, uint> TopCardTable
            = JsonSerializer.Deserialize<Dictionary<uint, uint>>(
                File.ReadAllText("./Data/topCardTable.json"));
    }
}
