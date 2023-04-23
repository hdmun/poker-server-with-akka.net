using Domain.PokerRule.Extentions;
using System;
using Xunit;

namespace Domain.PokerRule.Tests.Extentions
{
    public class ArrayExtentionsTests
    {
        [Fact]
        public void ShuffleTest()
        {
            var array  = new[] { 1, 2, 3, 4, 5 };
            var shuffled = array.Shuffle();

            Assert.Equal(shuffled.Length, array.Length);
            Assert.Equal(array, shuffled);
        }

        [Fact]
        public void IsInBoundsTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            Assert.True(array.IsInBounds(0));
            Assert.True(array.IsInBounds(1));
            Assert.True(array.IsInBounds(2));
            Assert.True(array.IsInBounds(3));
            Assert.True(array.IsInBounds(4));
            Assert.False(array.IsInBounds(5));
        }

        [Fact]
        public void FindIndexTest()
        {
            var array = new[] { 1, 2, 2, 4, 5 };
            var match = new Predicate<int>(x => x == 2);

            Assert.Equal(1, array.FindIndex(match));
            Assert.Equal(2, array.FindIndex(2, match));
            Assert.Equal(-1, array.FindIndex(3, match));
            Assert.Equal(1, array.FindIndex(0, 3, match));
            Assert.Equal(-1, array.FindIndex(0, 3, x => x == 5));
        }

        [Fact]
        public void ForEachFullyTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var enumerator = array.ForEachFully(2)
                .GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(4, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(5, enumerator.Current);

            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
        }
    }
}
