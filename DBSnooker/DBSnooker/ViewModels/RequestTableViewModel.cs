using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSnooker.ViewModels
{
    public class RequestTableViewModel : ViewModelBase
    {
        private List<List<object>> reqList;
        private List<Dictionary<string, object?>> reqDictionaries;
        public RequestTableViewModel(List<Dictionary<string, object?>> _reqDict)
        {
            reqDictionaries = _reqDict;
            reqList = new List<List<object>>();

            List<string> properties = new List<string>();

            // first elem - title of column
            foreach (var property in _reqDict[0])
            {
                properties.Add(property.Key);
            }

            // dictionary into prop list of every column
            foreach (string property in properties)
            {
                List<object> values = new List<object>();
                values.Add(property + "    ");
                values.Add(" ");
                foreach (Dictionary<string, object?> item in _reqDict)
                {
                    values.Add(item[property]);
                }
                reqList.Add(values);
            }
        }

        public List<List<object>> QueryList
        {
            get
            {
                return reqList;
            }
        }

        public override List<Dictionary<string, object?>> GetRows()
        {
            return reqDictionaries;
        }
    }
}
