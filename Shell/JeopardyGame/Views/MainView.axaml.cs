using Avalonia;
using Avalonia.Controls;
using Notification.Module.Services;
using Prism.Ioc;

namespace JeopardyGame.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            // Initialize the WindowNotificationManager with the "TopLevel". Previously (v0.10), MainWindow
            INotificationService? notifyService = ContainerLocator.Current.Resolve<INotificationService>();
            notifyService.SetHostWindow(TopLevel.GetTopLevel(this));
        }
    }
}