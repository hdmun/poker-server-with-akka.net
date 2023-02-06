using Domain.Poker.Simulator.Model;
using Domain.PokerRule.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Poker.Simulator.Tests
{
    public class HandEvaluatorTest
    {
        [Fact]
        public void Evaluate_StraightFlush_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Queen, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Five, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.StraightFlush, handValue.Rank);
            Assert.Equal(10UL, handValue.Priority);
            Assert.Equal(0UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_Flush_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Queen, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Eight, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Five, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Clubs).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.Flush, handValue.Rank);
            Assert.Equal(0UL, handValue.Priority);
            Assert.Equal(0b1_1100_0100_1001UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_Straight_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Queen, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Five, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.Straight, handValue.Rank);
            Assert.Equal(10UL, handValue.Priority);
            Assert.Equal(0UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_FullHouse_Test()
        {
            // given
            ulong cardsMask = new Card[]
            {
                Card.Get(Rank.Ace, Shape.Clubs),
                Card.Get(Rank.Ace, Shape.Diamonds),
                Card.Get(Rank.Ace, Shape.Hearts),
                Card.Get(Rank.King, Shape.Clubs),
                Card.Get(Rank.King, Shape.Spades),
                Card.Get(Rank.King, Shape.Hearts),
                Card.Get(Rank.Two, Shape.Hearts),
            }.Aggregate(0UL, (bitMask, card) => bitMask |= card.ToBitMask());

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(cardsMask);

            // then
            Assert.Equal(HandRank.FullHouse, handValue.Rank);
            Assert.Equal(0b1_0000_0000_0000UL, handValue.Priority);
            Assert.Equal(0b0_1000_0000_0000UL, handValue.Kickers);
        }


        public static IEnumerable<object[]> FourOfAKindData()
            => new Card[][]{
                new Card[] {
                    Card.Get(Rank.King, Shape.Diamonds),
                    Card.Get(Rank.King, Shape.Clubs),
                    Card.Get(Rank.King, Shape.Spades),
                    Card.Get(Rank.King, Shape.Hearts),
                    Card.Get(Rank.Ace, Shape.Hearts),
                    Card.Get(Rank.Queen, Shape.Hearts),
                    Card.Get(Rank.Ten, Shape.Clubs),
                },
                new Card[] {
                    Card.Get(Rank.King, Shape.Diamonds),
                    Card.Get(Rank.King, Shape.Clubs),
                    Card.Get(Rank.King, Shape.Spades),
                    Card.Get(Rank.King, Shape.Hearts),
                    Card.Get(Rank.Ace, Shape.Hearts),
                    Card.Get(Rank.Ten, Shape.Spades),
                    Card.Get(Rank.Ten, Shape.Clubs),
                }
            };

        [Theory]
        [MemberData(nameof(FourOfAKindData))]
        public void Evaluate_FourOfAKind_Test(Card h1, Card h2, Card flop1, Card flop2, Card flop3, Card turn, Card river)
        {
            // given
            ulong cardsMask = new Card[] { h1, h2, flop1, flop2, flop3, turn, river }
                .Aggregate(0UL, (bitMask, card) => bitMask |= card.ToBitMask());

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(cardsMask);

            // then
            Assert.Equal(HandRank.FourOfAKind, handValue.Rank);
            Assert.Equal(0b0_1000_0000_0000UL, handValue.Priority);
            Assert.Equal(0b1_0000_0000_0000UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_Trips_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Ten, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Five, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.Trips, handValue.Rank);
            Assert.Equal(0b0_0001_0000_0000UL, handValue.Priority);
            Assert.Equal(0b1_1000_0000_0000UL, handValue.Kickers);
        }

        public static IEnumerable<object[]> TwoPairData()
        {
            yield return new Card[] {
                Card.Get(Rank.Ace, Shape.Diamonds),
                Card.Get(Rank.Ace, Shape.Hearts),
                Card.Get(Rank.King, Shape.Clubs),
                Card.Get(Rank.King, Shape.Hearts),
                Card.Get(Rank.Queen, Shape.Diamonds),
                Card.Get(Rank.Five, Shape.Clubs),
                Card.Get(Rank.Two, Shape.Hearts),
            };
            yield return new Card[] {
                Card.Get(Rank.Ace, Shape.Diamonds),
                Card.Get(Rank.Ace, Shape.Hearts),
                Card.Get(Rank.King, Shape.Clubs),
                Card.Get(Rank.King, Shape.Diamonds),
                Card.Get(Rank.Two, Shape.Hearts),
                Card.Get(Rank.Two, Shape.Spades),
                Card.Get(Rank.Queen, Shape.Clubs)
            };
        }

        [Theory]
        [MemberData(nameof(TwoPairData))]
        public void Evaluate_TwoPair_Test(Card h1, Card h2, Card flop1, Card flop2, Card flop3, Card turn, Card river)
        {
            // given
            ulong cardsMask = new Card[] { h1, h2, flop1, flop2, flop3, turn, river }
                .Aggregate(0UL, (bitMask, card) => bitMask |= card.ToBitMask());

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(cardsMask);

            // then
            Assert.Equal(HandRank.TwoPair, handValue.Rank);
            Assert.Equal(0b1_1000_0000_0000UL, handValue.Priority);
            Assert.Equal(0b0_0100_0000_0000UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_Pair_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Ace, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Queen, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Five, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.Pair, handValue.Rank);
            Assert.Equal(0b1_0000_0000_0000UL, handValue.Priority);
            Assert.Equal(0b0_1101_0000_0000UL, handValue.Kickers);
        }

        [Fact]
        public void Evaluate_HighCard_Test()
        {
            // given
            ulong shared = Card.Get(Rank.Queen, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Nine, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Five, Shape.Clubs).ToBitMask()
                | Card.Get(Rank.Two, Shape.Hearts).ToBitMask();
            ulong playerCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask();

            // when
            var evaluator = HandEvaluator.From();
            var handValue = evaluator.Evaluate(shared | playerCards);

            // then
            Assert.Equal(HandRank.HighCard, handValue.Rank);
            Assert.Equal(0UL, handValue.Priority);
            Assert.Equal(0b1_1101_1000_0000UL, handValue.Kickers); ;
        }
    }
}
