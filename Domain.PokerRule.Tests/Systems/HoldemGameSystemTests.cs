using Domain.PokerRule.Systems;
using Xunit;

namespace Domain.PokerRule.Tests.Systems
{
    public class HoldemGameSystemTests
    {
        [Fact]
        public void InitializeTest()
        {
            var sys = new HoldemGameSystem();

            sys.Initialize();

            Assert.Equal(10, sys.HoldemTables.Count);
        }

        [Fact]
        public void AddPlayerTest()
        {
            var sys = new HoldemGameSystem();
            sys.Initialize();

            int tableId = 1;
            (int p1Id, int p1Pos) = (1, 0);
            (int p2Id, int p2Pos) = (2, 1);

            var result1 = sys.AddPlayer(tableId, p1Id, p1Pos);
            var result2 = sys.AddPlayer(tableId, p2Id, p2Pos);

            Assert.Equal(10, sys.HoldemTables.Count);
            Assert.True(result1);
            Assert.True(result2);
        }
    }
}
