using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;


namespace DBSnooker.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual object getThisTable() { return null; }
        public bool RemoveInProgress { get; set; } = false;
        public List<object>? RemovableItems { get; set; }

        public virtual List<Dictionary<string, object?>> GetRows()
        {
            return new List<Dictionary<string, object?>>();
        }
    }
}
