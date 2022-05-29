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
    public class GroupHandler : Handler
    {
        public GroupHandler(RequestManagerViewModel _RequestManager)
        {
            RM = _RequestManager;
        }

        public override void Try()
        {
            if (RM != null)
            {
                if (RM.GroupingColumn != null)
                {
                    try
                    {
                        var result = RM.ResultTable.GroupBy(item => item[RM.GroupingColumn]).ToList();
                        RM.ResultTable.Clear();
                        foreach (var group in result)
                        {
                            foreach (Dictionary<string, object?> row in group)
                            {
                                RM.ResultTable.Add(row);
                            }
                        }

                        if (RM.ResultTable.Count != 0)
                        {
                            RM.IsRequestSuccess = true;

                            if (NextHope != null)
                            {
                                NextHope.Try();
                            }
                        }
                        else
                        {
                            RM.IsRequestSuccess = false;
                            return;
                        }
                    }
                    catch
                    {
                        RM.IsRequestSuccess = false;
                        return;
                    }
                }
                else if (RM.GroupFilters.Count == 1 && RM.GroupFilters[0].FilterVal == ""
                        && RM.GroupFilters[0].Operator == "" && RM.GroupFilters[0].Column == "")
                {
                    RM.IsRequestSuccess = true;
                    return;
                }
                else
                {
                    RM.IsRequestSuccess = false;
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}
