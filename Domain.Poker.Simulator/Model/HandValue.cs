using Domain.PokerRule.Data;

namespace Domain.Poker.Simulator.Model
{
    public readonly struct HandValue
    {
        public static HandValue Of(HandRank handRank, uint kickers, uint topCardMasks)
            => new(handRank, kickers, topCardMasks);

        private readonly byte _rank;
        private readonly uint _priority;
        private readonly uint _kickers;

        private HandValue(HandRank handRank, uint kickers, uint topCardMasks)
        {
            _rank = (byte)handRank;
            _priority = kickers;
            _kickers = topCardMasks;
        }

        public HandRank Rank { get => (HandRank)_rank; }
        public uint Priority { get => _priority; }
        public uint Kickers { get => _kickers; }
    }
}
