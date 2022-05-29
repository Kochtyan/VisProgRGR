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
    public class RequestManagerViewModel : ViewModelBase
    {
        // list of refs
        private ObservableCollection<Table> CollectionTables;  
        private ObservableCollection<Table> CollectionAllTables;
        private ObservableCollection<Table> CollectionRequests;
        private ObservableCollection<string> CollectionColumnList;      // list of available column titles for request
        private ObservableCollection<Filter> CollectionFilters;
        private ObservableCollection<Filter> CollectionGroupFilters;
        private MainWindowViewModel m_mainWindow;               // ref for main window
        private bool isBDTableSelected;

        internal Dictionary<string, string> Keys = new Dictionary<string, string>()
        {
            { "Player Id", "Player Id"},
            { "Game Id", "Game Id"},
            { "Event Id", "Event Id"},
        };  // dictionary of keys
        public RequestManagerViewModel(DataBaseViewModel _dataBaseView, MainWindowViewModel _mainWindow)
        {
            dataBaseView = _dataBaseView;
            m_mainWindow = _mainWindow;
            CollectionTables = dataBaseView.Tables;
            CollectionAllTables = dataBaseView.AllTables;
            CollectionRequests = new ObservableCollection<Table>();
            CollectionFilters = new ObservableCollection<Filter>();
            CollectionGroupFilters = new ObservableCollection<Filter>();
            CollectionColumnList = new ObservableCollection<string>();
            SelectedTables = new ObservableCollection<Table>();
            SelectedColumns = new ObservableCollection<string>();

            ResultTable = new List<Dictionary<string, object?>>();
            JoinedTable = new List<Dictionary<string, object?>>();
            SelectedColumnsTable = new List<Dictionary<string, object?>>();

            FilterChain = new FilterHandler(this, "Filters");
            GroupChain = new GroupHandler(this);
            GroupFilterChain = new FilterHandler(this, "GroupFilters");

            FilterChain.NextHope = GroupChain;
            GroupChain.NextHope = GroupFilterChain;

            IsRequestSuccess = false;
            isBDTableSelected = true;
        }
        public void UpdateColumnList()
        {
            ColumnList = new ObservableCollection<string>();
            if (JoinedTable.Count != 0)
            {
                foreach (var column in JoinedTable[0])
                {
                    ColumnList.Add(column.Key);
                }
            }
            Filters.Clear();
            GroupFilters.Clear();
        }
        public void AddRequest(string tableName)
        {
            FilterChain.Try();

            // if req successful - add table in all
            if (IsRequestSuccess)
            {
                ObservableCollection<string> properties = new ObservableCollection<string>();

                foreach (var item in ResultTable[0])
                {
                    properties.Add(item.Key);
                }

                Requests.Add(new Table(tableName, true, new RequestTableViewModel(ResultTable.ToList()), properties));
                AllTables.Add(Requests.Last());
            }

            IsBDTableSelected = true;

            ClearAll();

            m_mainWindow.OpenDataBaseViewer();
        }
        public void DeleteRequests()
        {
            Requests = new ObservableCollection<Table>(Requests.Where(table => AllTables.Any(tables => tables.Title == table.Title)));
            GC.Collect();
        }
        public void ClearAll()
        {
            ResultTable.Clear();
            JoinedTable.Clear();
            SelectedColumnsTable.Clear();
            SelectedTables.Clear();
            SelectedColumns.Clear();
            Filters.Clear();
            GroupFilters.Clear();
            ColumnList.Clear();
        }
        private bool TryJoin(string key1, List<Dictionary<string, object?>> table2, string key2)
        {
            try
            {
                JoinedTable = JoinedTable.Join(
                    table2,
                    firstItem => firstItem[key1],
                    secondItem => secondItem[key2],
                    (firstItem, secondItem) =>
                    {
                        Dictionary<string, object?> resultItem = new Dictionary<string, object?>();
                        foreach (var item in firstItem)
                        {
                            resultItem.TryAdd(item.Key, item.Value);
                        }
                        foreach (var item in secondItem)
                        {
                            if (item.Key != key2)
                                resultItem.TryAdd(item.Key, item.Value);
                        }
                        return resultItem;
                    }
                    ).ToList();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public void Join()
        {
            // atleast 1 table
            if (SelectedTables.Count > 0)
            {
                var check = SelectedTables.Where(tab => tab.Title == "Events");
                if (check.Count() != 0)
                {
                    Table tmp = check.Last();
                    SelectedTables.Remove(check.Last());
                    SelectedTables.Add(tmp);
                }
                JoinedTable = new List<Dictionary<string, object?>>(SelectedTables[0].tableValues);

                // if more then 1 table
                if (SelectedTables.Count > 1)
                {
                    // ref for table which will be join with result
                    List<Dictionary<string, object?>> joiningTable;

                    bool success = false;
                    for (int i = 1; i < SelectedTables.Count; i++)
                    {
                        joiningTable = SelectedTables[i].tableValues;

                        foreach (var keysPair in Keys)
                        {
                            //key1 to key2
                            success = TryJoin(keysPair.Key, joiningTable, keysPair.Value);
                            if (success)
                                break;

                            //key2 to key1
                            else
                            {
                                success = TryJoin(keysPair.Value, joiningTable, keysPair.Key);
                                if (success)
                                    break;
                            }
                        }
                        if (!success)
                        {
                            JoinedTable.Clear();
                            ResultTable = JoinedTable;
                            UpdateColumnList();
                            return;
                        }
                    }
                }
                UpdateColumnList();
                ResultTable = JoinedTable;
            }

            // if 0 tables
            else
            {
                JoinedTable.Clear();
                ResultTable = JoinedTable;
                ColumnList.Clear();
                IsBDTableSelected = true;
            }
        }
        public void Select()
        {
            SelectedColumnsTable = JoinedTable.Select(item =>
            {
                return new Dictionary<string, object?>(item.Where(property => SelectedColumns.Any(column => column == property.Key)));
            }).ToList();
            ResultTable = SelectedColumnsTable;
        }

        public bool IsRequestSuccess { get; set; }
        public bool IsBDTableSelected
        {
            get => isBDTableSelected;
            set
            {
                this.RaiseAndSetIfChanged(ref isBDTableSelected, value);
            }
        }
        public string? GroupingColumn { get; set; } = null;
        public List<Dictionary<string, object?>> ResultTable { get; set; }
        public List<Dictionary<string, object?>> JoinedTable { get; set; } 
        public List<Dictionary<string, object?>> SelectedColumnsTable { get; set; }
        public ObservableCollection<string> ColumnList
        {
            get => CollectionColumnList;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionColumnList, value);
            }
        }
        public ObservableCollection<string> SelectedColumns { get; set; }
        public ObservableCollection<Filter> Filters
        {
            get => CollectionFilters;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionFilters, value);
            }
        }
        public ObservableCollection<Filter> GroupFilters
        {
            get => CollectionGroupFilters;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionGroupFilters, value);
            }
        }
        public ObservableCollection<Table> Tables
        {
            get => CollectionTables;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionTables, value);
            }
        }
        public ObservableCollection<Table> SelectedTables { get; set; } 
        public ObservableCollection<Table> AllTables
        {
            get => CollectionAllTables;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionAllTables, value);
            }
        }
        public ObservableCollection<Table> Requests
        {
            get => CollectionRequests;
            set
            {
                this.RaiseAndSetIfChanged(ref CollectionRequests, value);
            }
        }
        public FilterHandler FilterChain { get; set; }
        public GroupHandler GroupChain { get; set; }
        public FilterHandler GroupFilterChain { get; set; }
        public DataBaseViewModel dataBaseView { get; }  // ref dbview       
    }

}

