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
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;

namespace DBSnooker.ViewModels
{
    public class DataBaseViewModel : ViewModelBase
    {
        private ObservableCollection<Table> CollectionTables;
        private ObservableCollection<Table> CollectionAllTables;
        private ObservableCollection<Event> CollectionEvents;
        private ObservableCollection<Game> CollectionGames;
        private ObservableCollection<GameResult> CollectionGamesResults;
        private ObservableCollection<Player> CollectionPlayers;
        private bool currentTableIsSubtable; // req table?
        public ObservableCollection<Table> Tables
        {
            get => CollectionTables;
            set { this.RaiseAndSetIfChanged(ref CollectionTables, value); }
        }
        public ObservableCollection<Table> AllTables
        {
            get => CollectionAllTables;
            set { this.RaiseAndSetIfChanged(ref CollectionAllTables, value); }
        }
        public ObservableCollection<Event> Events
        {
            get => CollectionEvents;
            set { this.RaiseAndSetIfChanged(ref CollectionEvents, value); }
        }

        public ObservableCollection<Game> Games
        {
            get => CollectionGames;
            set { this.RaiseAndSetIfChanged(ref CollectionGames, value); }
        }

        public ObservableCollection<GameResult> GamesResults
        {
            get => CollectionGamesResults;
            set { this.RaiseAndSetIfChanged(ref CollectionGamesResults, value); }
        }

        public ObservableCollection<Player> Players
        {
            get => CollectionPlayers;
            set { this.RaiseAndSetIfChanged(ref CollectionPlayers, value); }
        }

        private ObservableCollection<string> searchField(string eTitle, List<string> fieldsList)
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            for (int i = 0; i < fieldsList.Count(); i++)
            {
                if (fieldsList[i].IndexOf("EntityType:" + eTitle) != -1)
                {
                    try
                    {
                        i++;
                        while (fieldsList[i].IndexOf("(") != -1 && i < fieldsList.Count())
                        {
                            result.Add(fieldsList[i].Remove(fieldsList[i].IndexOf("(")));
                            i++;
                        }
                        return result;
                    }
                    catch
                    {
                        return result;
                    }
                }
            }
            return result;
        }
        public DataBaseViewModel()
        {
            CollectionTables = new ObservableCollection<Table>();
            var DataBase = new SnookerDataBaseContext();
            CurrentTableName = "Events";
            CurrentTableIsSubtable = false;

            string tableInfo = DataBase.Model.ToDebugString();
            tableInfo = tableInfo.Replace(" ", "");

            string[] splitTableInfo = tableInfo.Split("\r\n");

            List<string> fieldlist = new List<string>(splitTableInfo); 

            AllTables = new ObservableCollection<Table>(Tables.ToList());


            DataBase.Events.Load<Event>();
            Events = DataBase.Events.Local.ToObservableCollection();
            CollectionTables.Add(new Table("Events", false, new EventTableViewModel(CollectionEvents), searchField("Event", fieldlist)));

            DataBase.Games.Load<Game>();
            Games = DataBase.Games.Local.ToObservableCollection();
            CollectionTables.Add(new Table("Games", false, new GameTableViewModel(CollectionGames), searchField("Game", fieldlist)));

            DataBase.GameResults.Load<GameResult>();
            GamesResults = DataBase.GameResults.Local.ToObservableCollection();
            CollectionTables.Add(new Table("GamesResults", false, new GameResultTableViewModel(CollectionGamesResults), searchField("GameResult", fieldlist)));

            DataBase.Players.Load<Player>();
            Players = DataBase.Players.Local.ToObservableCollection();
            CollectionTables.Add(new Table("Players", false, new PlayerTableViewModel(CollectionPlayers), searchField("Player", fieldlist)));
        }
        public void AddItem()
        {
            switch (CurrentTableName)
            {
                case "Games":
                    {
                        Games.Add(new Game());
                        break;
                    }
                case "GamesResults":
                    {
                        GamesResults.Add(new GameResult());
                        break;
                    }
                case "Events":
                    {
                        Events.Add(new Event());
                        break;
                    }
                case "Players":
                    {
                        Players.Add(new Player());
                        break;
                    }
            }
        }
        public bool CurrentTableIsSubtable
        {
            get => !currentTableIsSubtable;
            set => this.RaiseAndSetIfChanged(ref currentTableIsSubtable, value);
        }

        public SnookerDataBaseContext DataBase { get; set; }
        public string CurrentTableName { get; set; }

        public void DeleteItems()
        {
            Table currentTable = Tables.Where(table => table.Title == CurrentTableName).ToList()[0];

            List<object>? RemovableItems = currentTable.GetRemovableItems();

            currentTable.SetRemoveInProgress(true); // to stop datagrid 

            if (RemovableItems != null && RemovableItems.Count != 0)
            {
                switch (CurrentTableName)
                {
                    case "Games":
                        {
                            for (int i = 0; i < RemovableItems.Count; i++)
                            {
                                Games.Remove(RemovableItems[i] as Game);
                            }
                            break;
                        }
                    case "GamesResults":
                        {
                            for (int i = 0; i < RemovableItems.Count; i++)
                            {
                                GamesResults.Remove(RemovableItems[i] as GameResult);
                            }
                            break;
                        }
                    case "Events":
                        {
                            for (int i = 0; i < RemovableItems.Count; i++)
                            {
                                Events.Remove(RemovableItems[i] as Event);
                            }
                            break;
                        }
                    case "Players":
                        {
                            for (int i = 0; i < RemovableItems.Count; i++)
                            {
                                Players.Remove(RemovableItems[i] as Player);
                            }
                            break;
                        }
                }
            }
            currentTable.SetRemoveInProgress(false);
        }
        public void Save()
        {
            DataBase.SaveChanges();
        }
    }

}
