using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using DBSnooker.Models;
using Microsoft.Data.Sqlite;
using System.IO;
using System;

namespace DBSnooker.ViewModels
{
    public class EventTableViewModel : ViewModelBase
    {
        private ObservableCollection<Event> table;
        public ObservableCollection<Event> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public EventTableViewModel(ObservableCollection<Event> Collection)
        {
            thisTable = Collection;
            RemovableItems = new List<object>();
        }

        public override ObservableCollection<Event> getThisTable()
        {
            return thisTable;
        }
    }
}
