using System;
using System.Collections.Generic;

namespace DBSnooker
{
    public partial class GameResult
    {
        public long? GameId { get; set; }
        public long? Score1 { get; set; }
        public long? Score2 { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Game Id": return GameId;
                    case "Score1": return Score1;
                    case "Score2": return Score2;
                }
                return null;
            }
        }

        public virtual Game? Game { get; set; }
    }
}
