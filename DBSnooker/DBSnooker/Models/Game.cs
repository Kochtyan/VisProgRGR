using System;
using System.Collections.Generic;

namespace DBSnooker
{
    public partial class Game
    {
        public Game()
        {
            GameId = 0;
            EventId = 0;
            PlayerName = "None";
            Opponent = "None";
            Stage = "None";
        }
        public long GameId { get; set; }
        public long? EventId { get; set; }
        public string? PlayerName { get; set; }
        public string? Opponent { get; set; }
        public string? Stage { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Game Id": return GameId;
                    case "Event Name": return EventId;
                    case "Player Name": return PlayerName;
                    case "Opponent": return Opponent;
                    case "Stage": return Stage;
                }
                return null;
            }
        }
        public string Key()
        {
            return "Game Id";
        }

        public virtual Event? Event { get; set; }
    }
}
