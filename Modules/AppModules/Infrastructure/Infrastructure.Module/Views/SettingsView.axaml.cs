using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Infrastructure.Module.Views
{
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools(KeyGesture.Parse("Shift+F12"));
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Closing += (s, e) =>
            {
                ((Window)s)?.Hide();
                e.Cancel = true;
            };
        }
        
    }
}