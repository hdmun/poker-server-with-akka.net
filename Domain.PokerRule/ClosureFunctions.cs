using Domain.PokerRule.Data;
using Domain.PokerRule.Extentions;
using System;
using System.Linq;

namespace Domain.PokerRule
{
    public static class ClosureFunctions
    {
        public static Func<T[], int> GetFuncArrayNextIndex<T>(int initIndex, Predicate<T> match)
        {
            var NotNull = new Predicate<T>(x => x != null);

            int currentIdx = initIndex;
            return (array) =>
            {
                var nextIdx = currentIdx + 1;
                var idx = array.FindIndex(nextIdx, match);
                if (idx < 0)
                    idx = array.FindIndex(0, currentIdx, match);

                currentIdx = idx;
                return idx;
            };
        }

        public static Func<T> GetFuncArrayShuffler<T>(T[] array)
        {
            var shuffleIdx = Enumerable.Range(0, Card.DeckCount)
                .ToArray()
                .Shuffle()
                .AsEnumerable()
                .GetEnumerator();

            return () =>
            {
                shuffleIdx.MoveNext();
                return array[shuffleIdx.Current];
            };
        }
    }
}
