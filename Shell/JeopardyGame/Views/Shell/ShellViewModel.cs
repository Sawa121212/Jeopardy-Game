using Prism.Mvvm;

namespace JeopardyGame.Views.Shell
{
    public partial class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
        }

        public string Title => "Своя игра";
    }
}