using System;
using System.Collections.Generic;

namespace DBSnooker
{
    public partial class Player
    {
        public long PlayerId { get; set; }
        public string PlayerName { get; set; } = null!;
        public string? PlayerCountry { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Player Id": return PlayerId;
                    case "Player Name": return PlayerName;
                    case "Player Country": return PlayerCountry;
                }
                return null;
            }
        }
    }
}
