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
    internal class GameResultTableViewModel : ViewModelBase
    {
        private ObservableCollection<GameResult> table;
        public ObservableCollection<GameResult> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public GameResultTableViewModel(ObservableCollection<GameResult> Collection)
        {
            thisTable = Collection;
            RemovableItems = new List<object>();
        }

        public override ObservableCollection<GameResult> getThisTable()
        {
            return thisTable;
        }
    }
}
