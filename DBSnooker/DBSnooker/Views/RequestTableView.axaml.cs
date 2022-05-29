using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBSnooker.Views
{
    public partial class RequestTableView : UserControl
    {
        public RequestTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
