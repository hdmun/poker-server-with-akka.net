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

        public static bool operator ==(HandValue leftHand, HandValue rightHand)
        {
            return leftHand.Rank == rightHand.Rank
                && leftHand.Priority == rightHand.Priority
                && leftHand.Kickers == rightHand.Kickers;
        }

        public static bool operator !=(HandValue leftHand, HandValue rightHand)
        {
            return !(leftHand == rightHand);
        }

        public static bool operator >(HandValue leftHand, HandValue rightHand)
        {
            if (leftHand.Rank > rightHand.Rank)
                return true;
            else if (leftHand.Rank < rightHand.Rank)
                return false;

            if (leftHand.Priority > rightHand.Priority)
                return true;
            else if (leftHand.Priority < rightHand.Priority)
                return false;

            if (leftHand.Kickers > rightHand.Kickers)
                return true;
            else if (leftHand.Kickers < rightHand.Kickers)
                return false;

            return false;
        }

        public static bool operator <(HandValue leftHand, HandValue rightHand)
        {
            return !(leftHand > rightHand);
        }
    }
}
