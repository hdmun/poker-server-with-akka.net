using System;

namespace Domain.PokerRule.Extentions
{
    public static class ArrayExtentions
    {
        private static readonly Random random = new Random();

        public static T[] Shuffle<T>(this T[] array)
        {
            // Fisher-Yates Shuffle Alogorithm
            var count = array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                var randIdx = random.Next(0, count);
                var tmp = array[randIdx];
                array[randIdx] = array[i];
                array[i] = tmp;
            }
            return array;
        }

        public static bool IsInBounds<T>(this T[] array, int index)
        {
            return 0 <= index && index < array.Length;
        }

        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
            return Array.FindIndex(array, predicate);
        }
    }
}
