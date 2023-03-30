using System;

namespace Domain.PokerRule.Extentions
{
    public static class IntArrayExtentions
    {
        private static Random random = new Random();

        public static int[] Shuffle(this int[] array)
        {
            // Fisher-Yates Shuffle Alogorithm
            int count = array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                int randIdx = random.Next(0, count);
                int tmp = array[randIdx];
                array[randIdx] = array[i];
                array[i] = tmp;
            }
            return array;
        }
    }
}
