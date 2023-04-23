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
        public void AddAndRemovePlayerTest()
        {
            var sys = new HoldemGameSystem();
            sys.Initialize();

            int tableId = 1;
            (int p1Id, int p1Pos) = (1, 0);
            (int p2Id, int p2Pos) = (2, 1);

            var resultAdd1 = sys.AddPlayer(tableId, p1Id, p1Pos);
            var resultAdd2 = sys.AddPlayer(tableId, p2Id, p2Pos);

            Assert.True(resultAdd1);
            Assert.True(resultAdd2);

            var resultRemove1 = sys.RemovePlayer(tableId, p1Id);
            var resultRemove2 = sys.RemovePlayer(tableId, p2Id);

            Assert.True(resultRemove1);
            Assert.True(resultRemove2);
        }

        [Fact]
        public void StartTest()
        {
            var sys = new HoldemGameSystem();
            sys.Initialize();

            int tableId = 1;
            (int p1Id, int p1Pos) = (1, 0);
            (int p2Id, int p2Pos) = (2, 1);

            var resultAdd1 = sys.AddPlayer(tableId, p1Id, p1Pos);
            var resultAdd2 = sys.AddPlayer(tableId, p2Id, p2Pos);

            Assert.True(resultAdd1);
            Assert.True(resultAdd2);

            sys.Start(tableId);
        }
    }
}
