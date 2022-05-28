using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBSnooker.ViewModels;

namespace DBSnooker.Views
{
    public partial class EventTableView : UserControl
    {
        public EventTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void Delete(object control, DataGridAutoGeneratingColumnEventArgs args)
        {
            if (args.PropertyName == "Item" || args.PropertyName == "Games")
            {
                args.Cancel = true;
            }
        }
        private void RowSelected(object control, SelectionChangedEventArgs args)
        {
            DataGrid? grid = control as DataGrid;
            ViewModelBase? context = this.DataContext as ViewModelBase;
            if (grid != null && context != null)
            {
                if (context.RemoveInProgress)
                    return;
                context.RemovableItems.Clear();
                foreach (object item in grid.SelectedItems)
                {
                    context.RemovableItems.Add(item);
                }
            }
        }
    }
}
