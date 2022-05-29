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
    public class Table
    {
        private string title;
        public bool checkTable { get; }
       public List<Dictionary<string, object?>> tableValues { get; }
        public ObservableCollection<string> fields { get; set; }
        private ViewModelBase thisTableView;

        public Table(string title, bool checkTable, ViewModelBase thisTableView, ObservableCollection<string> fields)
        {

            this.title = title;

            this.checkTable = checkTable;

            this.thisTableView = thisTableView;

            this.fields = fields;

            tableValues = new List<Dictionary<string, object?>>();

            dynamic myTable = TableView.getThisTable(); // list of elems of table

            // convert in dictionary
            if (myTable != null)
            {
                Key = myTable[0].Key();
                for (int j = 0; j < myTable.Count; j++)
                {
                    Dictionary<string, object?> tmp = new Dictionary<string, object?>();
                    foreach (string field in fields)
                    {
                        tmp.Add(field, myTable[j][field]);
                    }
                    tableValues.Add(tmp);
                }
            }
            // if reqtable get her fields
            else if (checkTable)
            {
                tableValues = TableView.GetRows();
            }
        }
        public string Key { get; set; }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public ViewModelBase TableView
        {
            get { return thisTableView; }
            set { thisTableView = value; }
        }
        public List<object>? GetRemovableItems()
        {
            return TableView.RemovableItems;
        }
        public void SetRemoveInProgress(bool value)
        {
            TableView.RemoveInProgress = value;
        }
    }
}
