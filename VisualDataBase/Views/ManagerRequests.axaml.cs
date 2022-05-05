using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VisualDataBase.Views
{
    public partial class ManagerRequests : Window
    {
        public ManagerRequests()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
