using Akka.Actor;
using Domain.PokerRule.Components;
using Server.Poker.Message;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Poker.Actors
{
    public class HoldemTableActor : ReceiveActor, IWithTimers
    {
        public ITimerScheduler Timers { get; set; }

        private readonly List<TablePlayerComponent> _players;
        private readonly HashSet<int> _watchers;

        public HoldemTableActor(int playerMaxCount)
        {
            _players = new List<TablePlayerComponent>(playerMaxCount);
            _watchers = new HashSet<int>();

            Receive<TableJoinMessage>(OnJoin);
            Receive<TableLeaveMessage>(OnLeave);
            Receive<TableSitInMessage>(OnSitIn);
            Receive<TableSitOutMessage>(OnSitOut);
            Receive<TableBuyInMessage>(OnBuyIn);
            Receive<TableBetMessage>(OnBet);
            Receive<TableFoldMessage>(OnFold);
            Receive<TableUpdateMessage>(OnUpdate);
            Receive<TableStartGameMessage>(OnStartGame);
        }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer("update", new TableUpdateMessage(), TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1));
        }

        private bool IsSit(int playerId)
            => _players.Any(player => player.Id == playerId);

        private bool IsWatcher(int playerId)
            => _watchers.Contains(playerId);

        private void OnJoin(TableJoinMessage message)
        {
            if (IsWatcher(message.PlayerId))
            {
                return;
            }

            _watchers.Add(message.PlayerId);
        }

        private void OnLeave(TableLeaveMessage message)
        {
            if (!IsWatcher(message.PlayerId))
            {
                return;
            }

            _watchers.Remove(message.PlayerId);
        }

        private void OnSitIn(TableSitInMessage message)
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

            _players[message.Position] = TablePlayerComponent.Of(message.PlayerId);
        }

        private void OnSitOut(TableSitOutMessage message)
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
        }

        private void OnBuyIn(TableBuyInMessage message)
        {
            // check action player
            // todo: check user stack
        }

        private void OnBet(TableBetMessage message)
        {
            // check action player
            // todo: check minimum bet size
            // todo: is raise?
        }

        private void OnFold(TableFoldMessage message)
        {
            // check position
            // check action player
        }

        private void OnUpdate(TableUpdateMessage message)
        {
            // is playing?
            // onStartGame
            // onPlayerActionTimeout
        }

        private void OnStartGame(TableStartGameMessage message)
        {
        }
    }
}
