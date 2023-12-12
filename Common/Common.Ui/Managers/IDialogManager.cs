using System.Threading.Tasks;
using Avalonia.Controls;
using Common.Ui.Views;

namespace Common.Ui.Managers
{
    /// <summary>
    /// Менеджер для работы с диалогами
    /// </summary>
    public interface IDialogManager
    {
        /// <summary>
        /// Показать окно со всеми плюшками.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        Task ShowDialog(Window window);

        /// <summary>
        /// Показать окно со всеми плюшками.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        //bool? ShowDialog<TParam>(Window window, TParam param);

        /// <summary>
        /// Показать диалоговое окно указанного типа.
        /// </summary>
        /// <typeparam name="TWindow">Тип представления.</typeparam>
        /// <returns>Результат выполнения диалогового окна.</returns>
        Task<bool?> ShowDialogAsync<TWindow>() where TWindow : Window;

        /// <summary>
        /// Показать диалоговое окно указанного типа.
        /// </summary>
        /// <typeparam name="TWindow">Тип представления.</typeparam>
        /// <typeparam name="TResult">Результат выполнения.</typeparam>
        /// <returns>Результат выполнения диалогового окна.</returns>
        /*bool? ShowDialog<TWindow, TResult>(out TResult result) where TWindow : Window; 

        /// <summary>
        /// Показать диалоговое окно указанного типа с параметрами.
        /// </summary>
        /// <typeparam name="TWindow">Тип представления.</typeparam>
        /// <typeparam name="TParam">Тип передаваемого параметра.</typeparam>
        /// <param name="param">Передаваемый параметр.</param>
        /// <returns>Результат выполнения диалогового окна.</returns>
        bool? ShowDialog<TWindow, TParam>(TParam param) where TWindow : Window;

        /// <summary>
        /// Показать диалоговое окно указанного типа с результатом
        /// </summary>
        /// <typeparam name="TWindow">Тип представления.</typeparam>
        /// <typeparam name="TResult">Результат</typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        TResult ShowDialog<TWindow, TResult>(Func<TWindow, TResult> selector)
            where TWindow : Window, IViewWithResult<TResult>;*/

        /// <summary>
        /// Показать диалоговое окно указанного типа с результатом
        /// </summary>
        /// <typeparam name="TWindow">Тип представления.</typeparam>
        /// <typeparam name="TResult">Результат</typeparam>
        /// <typeparam name="TParam">Передаваемый параметр.</typeparam>
        /// <returns></returns>
        Task<TResult> ShowDialogAsync<TWindow, TResult, TParam>(TParam param, bool canMinimize = false)
            where TWindow : Window, IViewWithResult<TResult>;

        TResult ShowDialogSync<TWindow, TResult, TParam>(TParam param, bool canMinimize = false)
            where TWindow : Window, IViewWithResult<TResult>;

        /// <summary>
        /// Получить ссылку на экземпляр основного окна приложения.
        /// </summary>
        /// <returns>Ссылка на экземпляр основного окна приложения.</returns>
        Window GetShellWindow();

        /// <summary>
        /// Создать новое представление указанного типа.
        /// </summary>
        /// <typeparam name="TView">Тип представления.</typeparam>
        /// <returns>Представление указанного типа.</returns>
        TView CreateView<TView>()
            where TView : Control;

        /// <summary>
        /// Закрыть главное окно.
        /// </summary>
        void CloseShell();

        /// <summary>
        /// Отключить у окна CloseWindowBehavior.
        /// </summary>
        /// <typeparam name="TWindow"></typeparam>
        /// <param name="window"></param>
        void RemoveCloseWindowBehavior<TWindow>(TWindow window) where TWindow : Window;
    }
}