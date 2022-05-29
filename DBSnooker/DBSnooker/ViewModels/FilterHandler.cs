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
    public class FilterHandler : Handler
    {
        private ObservableCollection<Filter> Filters { get; set; }
        private List<Dictionary<string, object?>> ResultTable { get; set; }
        public FilterHandler(RequestManagerViewModel _RequestManager, string collection)
        {
            RM = _RequestManager;
            ResultTable = new List<Dictionary<string, object?>>();

            if (collection == "Filters")
                Filters = RM.Filters;
            else
                Filters = RM.GroupFilters;
        }
        private bool SwitchForChain(Filter filter)
        {
            try
            {
                switch (filter.Operator)
                {
                    case ">":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) > double.Parse(filter.FilterVal)
                            ).ToList();
                            return true;
                        }
                    case "<":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) < double.Parse(filter.FilterVal)
                            ).ToList();
                            return true;
                        }
                    case "=":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            item[filter.Column].ToString() == filter.FilterVal
                            ).ToList();
                            return true;
                        }
                    case ">=":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) >= double.Parse(filter.FilterVal)
                            ).ToList();
                            return true;
                        }
                    case "<>":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            item[filter.Column].ToString() != filter.FilterVal
                            ).ToList();
                            return true;
                        }
                    case "<=":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) <= double.Parse(filter.FilterVal)
                            ).ToList();
                            return true;
                        }
                    case "In Range":
                        {
                            string separator = "..";
                            string[] range = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number >= double.Parse(range[0]) && number <= double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not In Range":
                        {
                            string separator = "..";
                            string[] range = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number < double.Parse(range[0]) || number > double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Contains":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.FilterVal.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) != -1 && value.IndexOf(filterVal) != 0)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Contains":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.FilterVal.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) == -1 || value.IndexOf(filterVal) == 0)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Is Null":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value == "None" || value == "0" || value == "0000-00-00 00:00:00"
                                   || value == "00:00" || value.Replace(" ", "") == "" || value.ToUpper() == "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Null":
                        {
                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value != "None" && value != "0" && value != "0000-00-00 00:00:00"
                                   && value != "00:00" && value.Replace(" ", "") != "" && value.ToUpper() != "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Belong":
                        {
                            string separator = ",";
                            string[] set = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                if (set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Belong":
                        {
                            string separator = ",";
                            string[] set = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            RM.ResultTable = RM.ResultTable.Where(item =>
                            {
                                if (!set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private bool SwitchForUnion(Filter filter)
        {
            try
            {
                switch (filter.Operator)
                {
                    case ">":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) > double.Parse(filter.FilterVal)
                            ).ToList()).ToList();
                            return true;
                        }
                    case "<":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) < double.Parse(filter.FilterVal)
                            ).ToList()).ToList();
                            return true;
                        }
                    case "=":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            item[filter.Column].ToString() == filter.FilterVal
                            ).ToList()).ToList();
                            return true;
                        }
                    case ">=":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) >= double.Parse(filter.FilterVal)
                            ).ToList()).ToList();
                            return true;
                        }
                    case "<>":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            item[filter.Column].ToString() != filter.FilterVal
                            ).ToList()).ToList();
                            return true;
                        }
                    case "<=":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) <= double.Parse(filter.FilterVal)
                            ).ToList()).ToList();
                            return true;
                        }
                    case "In Range":
                        {
                            string separator = "..";
                            string[] range = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number >= double.Parse(range[0]) && number <= double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not In Range":
                        {
                            string separator = "..";
                            string[] range = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number < double.Parse(range[0]) || number > double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Contains":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.FilterVal.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) != -1 && value.IndexOf(filterVal) != 0)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Contains":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.FilterVal.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) == -1 || value.IndexOf(filterVal) == 0)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Is Null":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value == "None" || value == "0" || value.Replace(" ", "") == "" || value.ToUpper() == "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Null":
                        {
                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value != "None" && value != "0" && value.Replace(" ", "") != "" && value.ToUpper() != "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Belong":
                        {
                            string separator = ",";
                            string[] set = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                if (set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Belong":
                        {
                            string separator = ",";
                            string[] set = filter.FilterVal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            ResultTable = ResultTable.Union(RM.ResultTable.Where(item =>
                            {
                                if (!set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private void ResultByUnion()
        {
            foreach (Filter filter in Filters)
            {
                if (filter.FilterVal.Replace(" ", "") == "" && filter.Operator != "Is Null" && filter.Operator != "Not Null")
                {
                    RM.IsRequestSuccess = false;
                    return;
                }
                else
                {
                    if (!SwitchForUnion(filter))
                    {
                        RM.IsRequestSuccess = false;
                        return;
                    }
                    else
                    {
                        RM.IsRequestSuccess = true;
                    }
                }
            }
            RM.ResultTable = ResultTable;
        }
        private void ResultByChain()
        {
            // if filter 1 or null - next hop
            if (Filters.Count == 1 && Filters[0].FilterVal == "" && Filters[0].Operator == "" && Filters[0].Column == "")
            {
                RM.IsRequestSuccess = true;
                return;
            }

            foreach (Filter filter in Filters)
            {
                if (filter.FilterVal.Replace(" ", "") == "" && filter.Operator != "Is Null" && filter.Operator != "Not Null")
                {
                    RM.IsRequestSuccess = false;
                    return;
                }
                else
                {
                    if (!SwitchForChain(filter))
                    {
                        RM.IsRequestSuccess = false;
                        return;
                    }
                    else
                    {
                        RM.IsRequestSuccess = true;
                    }
                }
            }
        }
        public override void Try()
        {
            if (RM != null)
            {
                if (Filters.Count != 0)
                {
                    if (Filters.Count > 1)
                    {
                        if (Filters[1].BoolOper == "AND")
                            ResultByChain();
                        else
                            ResultByUnion();
                    }
                    else
                    {
                        ResultByChain();
                    }
                    if (NextHope != null && RM.IsRequestSuccess != false)
                    {
                        NextHope.Try();
                    }
                }
                else if (NextHope != null && RM.SelectedColumns.Count != 0)
                {
                    NextHope.Try();
                }
                else
                    RM.IsRequestSuccess = true;
            }
            else
            {
                return;
            }
        }


    }
}
