using Domain.PokerRule.Data;
using Xunit;

namespace Domain.Poker.Simulator.Tests
{
    public class OddsSimulatorTest
    {
        [Fact]
        public void CalcOdds_HeadsUp_PreFlop_Test()
        {
            // given
            Hand[] playerHands = new Hand[] {
                Hand.Of(
                    Card.Get(Rank.Ace, Shape.Spades),
                    Card.Get(Rank.King, Shape.Clubs)),
                Hand.Of(
                    Card.Get(Rank.Seven, Shape.Hearts),
                    Card.Get(Rank.Two, Shape.Diamonds)),
            };

            var board = Board.From();
            var oddsSimulator = new OddsSimulator.Builder(
                HandCombination.Enumerable,
                HandEvaluator.From()
                ).Build();

            // when
            var reports = oddsSimulator.CalcOdds(playerHands, board);

            // then
            Assert.Equal(1_143_573, reports[0].WinCount);
            Assert.Equal(562_098, reports[0].LossCount);
            Assert.Equal(6_633, reports[0].TieCount);
            Assert.Equal(1_712_304, reports[0].TotalCount);

            Assert.Equal(562_098, reports[1].WinCount);
            Assert.Equal(1_143_573, reports[1].LossCount);
            Assert.Equal(6_633, reports[1].TieCount);
            Assert.Equal(1_712_304, reports[1].TotalCount);
        }

        [Fact]
        public void CalcOdds_HeadsUp_Flop_Test()
        {
            // given
            Hand[] playerHands = new Hand[] {
                Hand.Of(
                    Card.Get(Rank.Ace, Shape.Spades),
                    Card.Get(Rank.King, Shape.Clubs)),
                Hand.Of(
                    Card.Get(Rank.Seven, Shape.Hearts),
                    Card.Get(Rank.Two, Shape.Diamonds)),
            };

            var board = Board.From();
            board.OnFlop(new Card[] {
                Card.Get(Rank.Three, Shape.Spades),
                Card.Get(Rank.Five, Shape.Hearts),
                Card.Get(Rank.Seven, Shape.Diamonds),
            });

            var oddsSimulator = new OddsSimulator.Builder(
                HandCombination.Enumerable,
                HandEvaluator.From()
                ).Build();

            // when
            var reports = oddsSimulator.CalcOdds(playerHands, board);

            // then
            Assert.Equal(219, reports[0].WinCount);
            Assert.Equal(755, reports[0].LossCount);
            Assert.Equal(16, reports[0].TieCount);
            Assert.Equal(990, reports[0].TotalCount);
            Assert.Equal(reports[0].TotalCount, reports[1].WinCount + reports[1].LossCount + reports[1].TieCount);

            Assert.Equal(755, reports[1].WinCount);
            Assert.Equal(219, reports[1].LossCount);
            Assert.Equal(16, reports[1].TieCount);
            Assert.Equal(990, reports[1].TotalCount);
            Assert.Equal(reports[1].TotalCount, reports[1].WinCount + reports[1].LossCount + reports[1].TieCount);
        }

        [Fact]
        public void CalcOdds_HeadsUp_Turn_Test()
        {
            // given
            Hand[] playerHands = new Hand[] {
                Hand.Of(
                    Card.Get(Rank.Ace, Shape.Spades),
                    Card.Get(Rank.King, Shape.Clubs)),
                Hand.Of(
                    Card.Get(Rank.Seven, Shape.Hearts),
                    Card.Get(Rank.Two, Shape.Diamonds)),
            };

            var board = Board.From();
            board.OnFlop(new Card[] {
                Card.Get(Rank.Three, Shape.Spades),
                Card.Get(Rank.Five, Shape.Hearts),
                Card.Get(Rank.Seven, Shape.Diamonds),
            });
            board.OnRiver(Card.Get(Rank.Four, Shape.Hearts));

            var oddsSimulator = new OddsSimulator.Builder(
                HandCombination.Enumerable,
                HandEvaluator.From()
                ).Build();

            // when
            var reports = oddsSimulator.CalcOdds(playerHands, board);

            // then
            Assert.Equal(6, reports[0].WinCount);
            Assert.Equal(34, reports[0].LossCount);
            Assert.Equal(4, reports[0].TieCount);
            Assert.Equal(44, reports[0].TotalCount);
            Assert.Equal(reports[0].TotalCount, reports[1].WinCount + reports[1].LossCount + reports[1].TieCount);

            Assert.Equal(34, reports[1].WinCount);
            Assert.Equal(6, reports[1].LossCount);
            Assert.Equal(4, reports[1].TieCount);
            Assert.Equal(44, reports[1].TotalCount);
            Assert.Equal(reports[1].TotalCount, reports[1].WinCount + reports[1].LossCount + reports[1].TieCount);
        }
    }
}
