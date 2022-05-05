using Avalonia.Controls;
using Avalonia.Interactivity;

namespace VisualDataBase.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void openNewWindow(object sender, RoutedEventArgs e)
        {
            ManagerRequests newWin = new ManagerRequests();
            newWin.Show();
        }
    }
}
