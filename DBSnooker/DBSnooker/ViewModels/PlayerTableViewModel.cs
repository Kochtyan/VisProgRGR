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
    internal class PlayerTableViewModel : ViewModelBase
    {
        private ObservableCollection<Player> table;
        public ObservableCollection<Player> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public PlayerTableViewModel(ObservableCollection<Player> Collection)
        {
            thisTable = Collection;
            RemovableItems = new List<object>();
        }

        public override ObservableCollection<Player> getThisTable()
        {
            return thisTable;
        }
    }
}
