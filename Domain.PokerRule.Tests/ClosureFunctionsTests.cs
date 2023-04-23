using Domain.PokerRule.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace Domain.PokerRule.Tests
{
    public class ClosureFunctionsTests
    {
        [Fact]
        public void GetFuncArrayNextIndexTest()
        {
            int?[] array = new int?[] { 1, null, 3, null, 5 };
            var notNull = new Predicate<int?>(x => x != null);
            var getNextIndex = ClosureFunctions.GetFuncArrayNextIndex(0, notNull);

            var nextIdx1 = getNextIndex(array);
            var nextIdx2 = getNextIndex(array);
            var nextIdx3 = getNextIndex(array);

            Assert.Equal(2, nextIdx1);
            Assert.Equal(4, nextIdx2);
            Assert.Equal(0, nextIdx3);
        }

        [Fact]
        public void GetFuncArrayShufflerTest()
        {
            int[] array = new int[] { 1, 2, 3, 4, 5 };
            var getNext = ClosureFunctions.GetFuncArrayShuffler(array);

            var next1 = getNext();
            var next2 = getNext();
            var next3 = getNext();

            Assert.NotEqual(next1, next2);
            Assert.NotEqual(next2, next3);
            Assert.NotEqual(next3, next1);
        }
    }
}
