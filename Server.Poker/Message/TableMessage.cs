﻿namespace Server.Poker.Message
{
    public class TableJoinMessage
    {
        public int PlayerId { get; set; }
    }

    public class TableLeaveMessage
    {
        public int PlayerId { get; set; }
    }

    public class TableSitInMessage
    {
        public int PlayerId { get; set; }
        public int Position { get; set; }
    }

    public class TableSitOutMessage
    {
        public int PlayerId { get; set; }
    }

    public class TableBuyInMessage
    {
        public int PlayerId { get; set; }
        public int BuyInStack { get; set; }
    }

    public class TableBetMessage
    {
        public int BettingChip { get; set; }
    }

    public class TableFoldMessage
    {
    }

    public class TableUpdateMessage
    {
    }
    
    public class TableStartGameMessage
    {
    }
}
