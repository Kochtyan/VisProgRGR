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
using System.Reactive.Linq;

namespace DBSnooker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase page;
        private DataBaseViewModel ViewPage;
        private RequestManagerViewModel QueryPage;

        public ViewModelBase Page
        {
            set => this.RaiseAndSetIfChanged(ref page, value);
            get => page;
        }
        public MainWindowViewModel()
        {
            ViewPage = new DataBaseViewModel();
            QueryPage = new RequestManagerViewModel();
            Page = ViewPage;
        }
        public void OpenDBViewer()
        {
            Page = ViewPage;
        }
        public void OpenRequestManager()
        {
            Page = QueryPage;
        }
    }
}
