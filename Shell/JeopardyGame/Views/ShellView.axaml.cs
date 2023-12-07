using Avalonia;
using Avalonia.Controls;

namespace JeopardyGame.Views
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}