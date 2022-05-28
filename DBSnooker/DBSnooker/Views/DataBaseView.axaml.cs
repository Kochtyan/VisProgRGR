using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using DBSnooker.ViewModels;
using System;

namespace DBSnooker.Views
{
    public partial class DataBaseView : UserControl
    {
        public DataBaseView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void DeleteTab(object control, RoutedEventArgs args)
        {
            Button? btn = control as Button;
            if (btn != null)
            {
                DataBaseViewModel? context = this.DataContext as DataBaseViewModel;
                if (context != null)
                {
                    context.AllTables.Remove(btn.DataContext as Table);
                    GC.Collect();
                }
            }
        }
        private void SelectedTabChanged(object control, SelectionChangedEventArgs args)
        {
            TabControl? tabControl = control as TabControl;
            if (tabControl != null)
            {
                DataBaseViewModel? context = this.DataContext as DataBaseViewModel;
                Table? table = tabControl.SelectedItem as Table;
                if (context != null && table != null)
                {
                    context.CurrentTableName = table.Title;
                    context.CurrentTableIsSubtable = table.checkTable;
                }
            }
        }
    }
}
