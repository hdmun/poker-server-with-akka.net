using Domain.PokerRule.Data;
using System;
using System.Linq;

namespace Domain.Poker.Simulator
{
    public enum CardMaskFlag : ulong
    {
        Club2 = 1,
        Club3 = 1UL << 1,
        Club4 = 1UL << 2,
        Club5 = 1UL << 3,
        Club6 = 1UL << 4,
        Club7 = 1UL << 5,
        Club8 = 1UL << 6,
        Club9 = 1UL << 7,
        ClubT = 1UL << 8,
        ClubJ = 1UL << 9,
        ClubQ = 1UL << 10,
        ClubK = 1UL << 11,
        ClubA = 1UL << 12,
        Heart2 = 1UL << 13,
        Heart3 = 1UL << 14,
        Heart4 = 1UL << 15,
        Heart5 = 1UL << 16,
        Heart6 = 1UL << 17,
        Heart7 = 1UL << 18,
        Heart8 = 1UL << 19,
        Heart9 = 1UL << 20,
        HeartT = 1UL << 21,
        HeartJ = 1UL << 22,
        HeartQ = 1UL << 23,
        HeartK = 1UL << 24,
        HeartA = 1UL << 25,
        Diamond2 = 1UL << 26,
        Diamond3 = 1UL << 27,
        Diamond4 = 1UL << 28,
        Diamond5 = 1UL << 29,
        Diamond6 = 1UL << 30,
        Diamond7 = 1UL << 31,
        Diamond8 = 1UL << 32,
        Diamond9 = 1UL << 33,
        DiamondT = 1UL << 34,
        DiamondJ = 1UL << 35,
        DiamondQ = 1UL << 36,
        DiamondK = 1UL << 37,
        DiamondA = 1UL << 38,
        Spade2 = 1UL << 39,
        Spade3 = 1UL << 40,
        Spade4 = 1UL << 41,
        Spade5 = 1UL << 42,
        Spade6 = 1UL << 43,
        Spade7 = 1UL << 44,
        Spade8 = 1UL << 45,
        Spade9 = 1UL << 46,
        SpadeT = 1UL << 47,
        SpadeJ = 1UL << 48,
        SpadeQ = 1UL << 49,
        SpadeK = 1UL << 50,
        SpadeA = 1UL << 51,
    }

    public static class CardsMask
    {
        public static readonly int Count = Enum.GetValues(typeof(CardMaskFlag)).Length;

        // Cards + Shape * 13
        // 카드 인덱스 값을 bit flag mask 로 표현한 테이블
        public static readonly ulong[] Table
            = Enum.GetValues(typeof(CardMaskFlag)).Cast<ulong>().ToArray();

        public static int BitCount(ulong bitField)
        {
            // Divide and Conquer
            bitField = (bitField & 0x5555555555555555) + ((bitField >> 1) & 0x5555555555555555);
            bitField = (bitField & 0x3333333333333333) + ((bitField >> 2) & 0x3333333333333333);
            bitField = (bitField & 0x0f0f0f0f0f0f0f0f) + ((bitField >> 4) & 0x0f0f0f0f0f0f0f0f);
            bitField = (bitField & 0x00ff00ff00ff00ff) + ((bitField >> 8) & 0x00ff00ff00ff00ff);
            bitField = (bitField & 0x0000ffff0000ffff) + ((bitField >> 16) & 0x0000ffff0000ffff);
            bitField = (bitField & 0x00000000ffffffff) + ((bitField >> 32) & 0x00000000ffffffff);
            return (int)bitField;
        }

        private static readonly ulong OffsetBitFlag = 0x1fffUL;
        private static readonly int ClubsOffset = Enum.GetValues(typeof(Rank)).Length * (int)Shape.Clubs;
        private static readonly int HeartsOffset = Enum.GetValues(typeof(Rank)).Length * (int)Shape.Hearts;
        private static readonly int DiamondsOffset = Enum.GetValues(typeof(Rank)).Length * (int)Shape.Diamonds;
        private static readonly int SpadesOffset = Enum.GetValues(typeof(Rank)).Length * (int)Shape.Spades;

        private static uint RightShift(ulong mask, int shift)
            => (uint)((mask >> shift) & OffsetBitFlag);

        public static uint GetClubs(ulong mask)
            => RightShift(mask, ClubsOffset);

        public static uint GetHearts(ulong mask)
            => RightShift(mask, HeartsOffset);

        public static uint GetDiamonds(ulong mask)
            => RightShift(mask, DiamondsOffset);

        public static uint GetSpades(ulong mask)
            => RightShift(mask, SpadesOffset);
    }
}
