using System.Collections.Generic;
using System.Linq;

namespace Domain.PokerRule.Extentions
{
    public static class EnumerableExtentions
    {
        public static IEnumerable<T> ForEachFully<T>(this IEnumerable<T> source, int start)
        {
            var skipped = source.Skip(start);
            using (var e = skipped.GetEnumerator())
            {
                while (e.MoveNext())
                    yield return e.Current;
            }

            var take = source.Take(start);
            using (var e = take.GetEnumerator())
            {
                while (e.MoveNext())
                    yield return e.Current;
            }
        }
    }
}
