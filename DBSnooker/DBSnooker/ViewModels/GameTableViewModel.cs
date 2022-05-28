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
    internal class GameTableViewModel : ViewModelBase
    {
        private ObservableCollection<Game> table;
        public ObservableCollection<Game> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public GameTableViewModel(ObservableCollection<Game> Collection)
        {
            thisTable = Collection;
            RemovableItems = new List<object>();
        }

        public override ObservableCollection<Game> getThisTable()
        {
            return thisTable;
        }
    }
}
