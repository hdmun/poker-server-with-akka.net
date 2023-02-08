using Domain.PokerRule.Data;

namespace Domain.Poker.Simulator.Model
{
    public class HandOddsReport
    {
        public static HandOddsReport From(Hand hand)
            => new HandOddsReport(hand);

        public Hand Hand { get; private set; }
        public int WinCount { get; private set; }
        public int LossCount { get; private set; }
        public int TieCount { get; private set; }
        public int TotalCount { get; private set; }

        private HandOddsReport(Hand hand)
        {
            Hand = hand;
            WinCount = 0;
            LossCount = 0;
            TieCount = 0;
            TotalCount = 0;
        }

        public void Win()
        {
            WinCount++;
            TotalCount++;
        }

        public void Loss()
        {
            LossCount++;
            TotalCount++;
        }

        public void Tie()
        {
            TieCount++;
            TotalCount++;
        }
    }
}
