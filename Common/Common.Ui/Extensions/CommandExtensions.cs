using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Ui.Extensions
{
    public static class CommandExtensions
    {
        public static void Invoke(this ICommand command, object commandParameter = null)
        {
            if (command == null)
                return;

            if (command.CanExecute(commandParameter))
                command.Execute(commandParameter);
        }

        public static async Task InvokeAsync(this ICommand command, object commandParameter = null)
        {
            await Task.Run(() =>
            {
                if (command.CanExecute(commandParameter))
                    command.Execute(commandParameter);
            }).ConfigureAwait(true);
        }
    }
}