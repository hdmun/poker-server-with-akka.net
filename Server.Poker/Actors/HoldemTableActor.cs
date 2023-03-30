using Akka.Actor;
using Server.Poker.Message;
using System.Collections.Generic;
using System.Linq;

namespace Server.Poker.Actors
{
    public class HoldemTablePlayer
    {
        public HoldemTablePlayer(int id, IActorRef actor)
        {
            Id = id;
            Actor = actor;
        }
        public int Id { get; private set; }
        public IActorRef Actor { get; private set; }
    }

    public class HoldemTableActor : ReceiveActor
    {
        private readonly List<HoldemTablePlayer> _players;
        private readonly HashSet<int> _watchers;

        public HoldemTableActor(int playerMaxCount)
        {
            _players = new List<HoldemTablePlayer>(playerMaxCount);
            _watchers = new HashSet<int>();

            Receive<TableJoinMessage>(message =>
            {
                if (IsWatcher(message.PlayerId))
                {
                    return;
                }

                _watchers.Add(message.PlayerId);
            });

            Receive<TableLeaveMessage>(message =>
            {
                if (!IsWatcher(message.PlayerId))
                {
                    return;
                }

                _watchers.Remove(message.PlayerId);
            });

            Receive<TableSitInMessage>(message =>
            {
                if (!IsWatcher(message.PlayerId))
                {
                    return;
                }

                if (IsSit(message.PlayerId))
                {
                    return;
                }

                if (message.Position < 0 || _players.Count <= message.Position)
                {
                    return;
                }

                if (_players[message.Position] != null)
                {
                    return;
                }

                _watchers.Remove(message.PlayerId);

                var playerActor = Context.ActorOf(
                    Props.Create(() => new HoldemTablePlayerActor(message.PlayerId)));
                _players[message.Position] = new HoldemTablePlayer(message.PlayerId, playerActor);
            });

            Receive<TableSitOutMessage>(message =>
            {
                if (!IsSit(message.PlayerId))
                {
                    return;
                }

                var index = _players.FindIndex(player => player?.Id == message.PlayerId);
                if (index < 0)
                {
                    return;
                }

                var player = _players[index];
                _players[index] = null;

                _watchers.Add(message.PlayerId);
            });
        }

        private bool IsSit(int playerId)
            => _players.Any(player => player.Id == playerId);

        private bool IsWatcher(int playerId)
            => _watchers.Contains(playerId);
    }
}
