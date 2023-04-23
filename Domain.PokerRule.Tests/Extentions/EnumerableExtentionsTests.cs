using Domain.PokerRule.Extentions;
using System.Linq;
using Xunit;

namespace Domain.PokerRule.Tests.Extentions
{
    public class EnumerableExtentionsTests
    {
        [Fact]
        public void ForEachFullyTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var enumerable = array.Where(_ => true).ForEachFully(2);
            using (var enumertor = enumerable.GetEnumerator())
            {
                enumertor.MoveNext();
                Assert.Equal(3, enumertor.Current);

                enumertor.MoveNext();
                Assert.Equal(4, enumertor.Current);

                enumertor.MoveNext();
                Assert.Equal(5, enumertor.Current);

                enumertor.MoveNext();
                Assert.Equal(1, enumertor.Current);

                enumertor.MoveNext();
                Assert.Equal(2, enumertor.Current);
            }
        }
    }
}
