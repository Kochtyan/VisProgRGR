using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using DBSnooker.Models;
using Microsoft.Data.Sqlite;
using System.IO;
using System;

namespace DBSnooker.ViewModels
{
    public abstract class Handler
    {
        protected RequestManagerViewModel? RM { get; set; } = null; // req window
        public Handler? NextHope { get; set; } // next handler
        public abstract void Try(); // handler func
    }
}
