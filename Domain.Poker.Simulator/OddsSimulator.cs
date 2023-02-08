using Domain.Poker.Simulator.Model;
using Domain.PokerRule.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Poker.Simulator
{
    public class OddsSimulator
    {
        public delegate IEnumerable<ulong> HandCombination(ulong shared, ulong deadCards, int maxCardCount);

        public class Builder
        {
            private readonly HandCombination _handCombination;
            private readonly HandEvaluator _handEvaluator;

            public Builder(HandCombination handCombination, HandEvaluator handEvaluator)
            {
                _handCombination = handCombination;
                _handEvaluator = handEvaluator;
            }

            public OddsSimulator Build()
            {
                return new OddsSimulator(_handCombination, _handEvaluator);
            }
        }

        private readonly HandCombination _handCombination;
        private readonly HandEvaluator _handEvaluator;

        private OddsSimulator(HandCombination handCombination, HandEvaluator handEvaluator)
        {
            _handCombination = handCombination;
            _handEvaluator = handEvaluator;
        }

        public HandOddsReport[] CalcOdds(Hand[] playerHands, Board board)
        {
            ulong shared = board.ToBitMask();
            ulong[] handMasks = playerHands
                .Select(hand => hand.ToBitMask())
                .ToArray();
            ulong deadCardsMask = handMasks
                .Aggregate(0UL, (bitMask, handMask) => bitMask |= handMask);

            var handOddsReports = playerHands.Select(hand => HandOddsReport.From(hand)).ToArray();

            HandValue[] handValues = new HandValue[playerHands.Length];

            foreach (ulong boardCardMask in _handCombination(shared, deadCardsMask, Board.MaxCardCount))
            {
                var bestHand = _handEvaluator.Evaluate(boardCardMask | handMasks[0]);
                handValues[0] = bestHand;

                uint bestCount = 1;
                for (int i = 1; i < playerHands.Length; i++)
                {
                    handValues[i] = _handEvaluator.Evaluate(boardCardMask | handMasks[i]);
                    if (handValues[i] > bestHand)
                    {
                        bestHand = handValues[i];

                    }
                    else if (handValues[i] == bestHand)
                    {
                        bestCount++;
                    }
                }

                for (int i = 0; i < playerHands.Length; i++)
                {
                    if (handValues[i] == bestHand)
                    {
                        if (bestCount > 1)
                        {
                            handOddsReports[i].Tie();
                        }
                        else
                        {
                            handOddsReports[i].Win();
                        }
                    }
                    else if (handValues[i] < bestHand)
                    {
                        handOddsReports[i].Loss();
                    }
                    
                }
            }

            return handOddsReports;
        }
    }
}
