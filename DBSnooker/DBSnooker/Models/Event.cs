using System;
using System.Collections.Generic;

namespace DBSnooker
{
    public partial class Event
    {
        public Event()
        {
            Games = new HashSet<Game>();
            EventId = 0;
            Season = "None";
            EventName = "None";
            Status = "None";
            Category = "None";
            EventCountry = "None";
            Prize = 0;
        }

        public long EventId { get; set; }
        public string? Season { get; set; }
        public string? EventName { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public string? EventCountry { get; set; }
        public long? Prize { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Event Id": return EventId;
                    case "Season": return Season;
                    case "Event Name": return EventName;
                    case "Status": return Status;
                    case "Category": return Category;
                    case "Event Country": return EventCountry;
                    case "Prize": return Prize;
                }
                return null;
            }
        }
        public string Key()
        {
            return "Event Id";
        }

        public virtual ICollection<Game> Games { get; set; }
    }
}
