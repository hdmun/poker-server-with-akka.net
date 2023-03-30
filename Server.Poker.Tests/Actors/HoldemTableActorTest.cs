using Akka.Actor;
using Akka.TestKit.Xunit;
using Server.Poker.Actors;
using Server.Poker.Message;
using Xunit;

namespace Server.Poker.Tests.Actors
{
    public class HoldemTableActorTest : TestKit
    {
        [Fact]
        public void JoinLeaveSuccessTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
            tableActor.Tell(new TableLeaveMessage() { PlayerId = playerId });
        }

        [Fact]
        public void JoinFailedTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
        }

        [Fact]
        public void LeaveFailedTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableLeaveMessage() { PlayerId = playerId });
            tableActor.Tell(new TableLeaveMessage() { PlayerId = playerId });
        }

        [Fact]
        public void SitInSuccessTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
            tableActor.Tell(new TableSitInMessage() { PlayerId = playerId });
        }

        [Fact]
        public void SitInFailedTest1()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableSitInMessage()
            {
                PlayerId = playerId,
                Position = 1
            });
        }

        [Fact]
        public void SitInFailedTest2()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
            tableActor.Tell(new TableSitInMessage()
            {
                PlayerId = playerId,
                Position = 1
            });
            tableActor.Tell(new TableSitInMessage()
            {
                PlayerId = playerId,
                Position = 1
            });
        }

        [Fact]
        public void SitOutSuccessTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableJoinMessage() { PlayerId = playerId });
            tableActor.Tell(new TableSitInMessage()
            {
                PlayerId = playerId,
                Position = 1
            });
            tableActor.Tell(new TableSitOutMessage() { PlayerId = playerId });
        }

        [Fact]
        public void SitOutFailedTest()
        {
            int playerId = 1;
            var tableActor = Sys.ActorOf(
                Props.Create(() => new HoldemTableActor(6)));

            tableActor.Tell(new TableSitOutMessage() { PlayerId = playerId });
        }
    }
}
