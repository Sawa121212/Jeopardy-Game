using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Views.Blocked
{
    public partial class BlockedView : UserControl
    {
        public BlockedView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}