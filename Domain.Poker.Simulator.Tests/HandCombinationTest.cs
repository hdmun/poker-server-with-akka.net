using Domain.PokerRule.Data;
using System;
using System.Linq;
using Xunit;

namespace Domain.Poker.Simulator.Tests
{
    public class HandCombinationTest
    {
        [Fact]
        public void preFlop_Combination_Test()
        {
            // given
            int maxCardCount = 5;
            ulong shared = 0UL;
            ulong deadCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Clubs).ToBitMask();

            // when
            int count = HandCombination.Enumerable(shared, deadCards, maxCardCount).Count();

            // then
            Assert.Equal(1712304, count);
        }

        [Fact]
        public void flop_Combination_Test()
        {
            // given
            int maxCardCount = 5;
            ulong shared = Card.Get(Rank.Two, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Three, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Four, Shape.Diamonds).ToBitMask();
            ulong deadCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Clubs).ToBitMask();

            // when
            int count = HandCombination.Enumerable(shared, deadCards, maxCardCount).Count();

            // then
            Assert.Equal(0UL, shared & deadCards);
            Assert.Equal(990, count);
        }

        [Fact]
        public void trun_Combination_Test()
        {
            // given
            int maxCardCount = 5;
            ulong shared = Card.Get(Rank.Two, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Three, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Four, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Nine, Shape.Diamonds).ToBitMask();
            ulong deadCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Clubs).ToBitMask();

            // when
            int count = HandCombination.Enumerable(shared, deadCards, maxCardCount).Count();

            // then
            Assert.Equal(0UL, shared & deadCards);
            Assert.Equal(44, count);
        }

        [Fact]
        public void Combination_PreFlop_Failed_Test()
        {
            // given
            int maxCardCount = 5;
            ulong shared = Card.Get(Rank.Two, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Three, Shape.Hearts).ToBitMask();
            ulong deadCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Clubs).ToBitMask();

            // when, then
            Assert.Equal(0UL, shared & deadCards);
            Assert.Throws<Exception>(() => HandCombination.Enumerable(shared, deadCards, maxCardCount));
        }

        [Fact]
        public void Combination_River_Failed_Test()
        {
            // given
            int maxCardCount = 5;
            ulong shared = Card.Get(Rank.Two, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Three, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Four, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Nine, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Three, Shape.Diamonds).ToBitMask();
            ulong deadCards
                = Card.Get(Rank.Ace, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.King, Shape.Hearts).ToBitMask()
                | Card.Get(Rank.Ten, Shape.Diamonds).ToBitMask()
                | Card.Get(Rank.Jack, Shape.Clubs).ToBitMask();

            // when, then
            Assert.Equal(0UL, shared & deadCards);
            Assert.Throws<Exception>(() => HandCombination.Enumerable(shared, deadCards, maxCardCount));
        }
    }
}
