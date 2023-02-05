using System;
using System.Collections.Generic;

namespace Domain.Poker.Simulator
{
    public static class HandCombination
    {
        public static IEnumerable<ulong> Enumerable(ulong shared, ulong deadCards, int maxCardCount)
        {
            deadCards |= shared;
            int sharedCardCount = CardsMask.BitCount(shared);
            int remainCardCount = maxCardCount - sharedCardCount;
            switch (remainCardCount)
            {
                case 5: // pre-flop
                    return preFlopLoop(shared, deadCards);
                case 2: // flop
                    return flopLoop(shared, deadCards);
                case 1: // trun
                    return combinationLoop(shared, deadCards);
                default:
                    throw new Exception($"invalid `reaminCardCount`: {remainCardCount}, maxCardCount: {maxCardCount}, sharedCardCount: {sharedCardCount}");
            }
        }

        private static IEnumerable<ulong> preFlopLoop(ulong shared, ulong deadCards)
        {
            for (int flop1Idx = CardsMask.Count - 1; flop1Idx >= 0; flop1Idx--)
            {
                ulong flop1Card = CardsMask.Table[flop1Idx];
                if ((deadCards & flop1Card) != 0) continue;

                for (int flop2Idx = flop1Idx - 1; flop2Idx >= 0; flop2Idx--)
                {
                    ulong flop2Card = CardsMask.Table[flop2Idx];
                    if ((deadCards & flop2Card) != 0) continue;

                    ulong flop12Cards = flop1Card | flop2Card;
                    for (int flop3Idx = flop2Idx - 1; flop3Idx >= 0; flop3Idx--)
                    {
                        ulong flop3Card = CardsMask.Table[flop3Idx];
                        if ((deadCards & flop3Card) != 0) continue;

                        ulong flopBoards = flop12Cards | flop3Card;
                        for (int trunIdx = flop3Idx - 1; trunIdx >= 0; trunIdx--)
                        {
                            ulong turnCard = CardsMask.Table[trunIdx];
                            if ((deadCards & turnCard) != 0) continue;

                            ulong turnBoards = flopBoards | turnCard;
                            for (int riverIdx = trunIdx - 1; riverIdx >= 0; riverIdx--)
                            {
                                ulong riverCard = CardsMask.Table[riverIdx];
                                if ((deadCards & riverCard) != 0) continue;

                                yield return turnBoards | riverCard | shared;
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<ulong> flopLoop(ulong shared, ulong deadCards)
        {
            for (int turnIndex = CardsMask.Count - 1; turnIndex >= 0; turnIndex--)
            {
                ulong trunCard = CardsMask.Table[turnIndex];
                if ((deadCards & trunCard) != 0) continue;

                for (int riverIndex = turnIndex - 1; riverIndex >= 0; riverIndex--)
                {
                    ulong riverCard = CardsMask.Table[riverIndex];
                    if ((deadCards & riverCard) != 0) continue;

                    yield return trunCard | riverCard | shared;
                }
            }
        }

        private static IEnumerable<ulong> combinationLoop(ulong shared, ulong deadCards)
        {
            for (int index = CardsMask.Count - 1; index >= 0; index--)
            {
                ulong cardMask = CardsMask.Table[index];
                if ((deadCards & cardMask) != 0) continue;

                yield return cardMask | shared;
            }
        }
    }
}
