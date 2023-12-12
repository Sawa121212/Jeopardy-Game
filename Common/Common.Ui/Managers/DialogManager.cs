using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Common.Core.Interfaces;
using Common.Ui.Parameters;
using Common.Ui.Views;
using DryIoc;
using Prism.Mvvm;
using ReactiveUI;

namespace Common.Ui.Managers
{
    public class DialogManager<TShellView> : IDialogManager where TShellView : Window
    {
        public DialogManager(IContainer container, IPresentationParameters presentationParameters)
        {
            _container = container;
            _presentationParameters = presentationParameters;
        }

        /// <inheritdoc />
        public async Task ShowDialog(Window window)
        {
            if (window.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(window);

            Task result = window.ShowDialog(window);

            if (window.DataContext is IClearable clearable)
                clearable.Clear();

            //return result;
        }

        /// <inheritdoc />
        /*public bool? ShowDialog<TParam>(Window window, TParam param)
        {
            if (!(window.DataContext is IInitializable<TParam> init))
                throw new InvalidOperationException("ViewModel must implement IInitializable<TParam>");
            init.Initialize(param);

            if (window.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(window);

            var result = window.ShowDialog(_shell);

            if (window.DataContext is IClearable clearable)
                clearable.Clear();

            return result;
        }*/

        /// <inheritdoc />
        public async Task<bool?> ShowDialogAsync<TWindow>() where TWindow : Window
        {
            TWindow view = CreateWindow<TWindow>();
            if (!(view.DataContext is BindableBase viewModel || view.DataContext is ReactiveObject)) // ObservableViewModelBase
                throw new InvalidOperationException("ViewModel must implement BindableBase");

            if (view.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(view);

            bool? result = await view.ShowDialog<bool?>(this.GetShellWindow());

            if (view.DataContext is IClearable clearable)
                clearable.Clear();

            return result;
        }

        /// <inheritdoc />
        /*public bool? ShowDialog<TWindow, TResult>(out TResult result) where TWindow : Window
        {
            result = default(TResult);

            var view = CreateWindow<TWindow>();
            if (!(view.DataContext is ObservableViewModelBase))
                throw new InvalidOperationException("ViewModel must implement ObservableViewModelBase");


            if (view.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(view);

            var dialogResult = view.ShowDialog();

            if (view.DataContext is IResult<TResult> resultable)
                result = resultable.GetResult();

            if (view.DataContext is IClearable clearable)
                clearable.Clear();

            return dialogResult;
        }*/

        /// <inheritdoc />
        /*public bool? ShowDialog<TWindow, TParam>(TParam param) where TWindow : Window
        {
            var view = CreateWindow<TWindow>();
            if (!(view.DataContext is ObservableViewModelBase))
                throw new InvalidOperationException("ViewModel must implement ObservableViewModelBase");

            if (!(view.DataContext is IInitializable<TParam> init))
                throw new InvalidOperationException("ViewModel must implement IInitializable<TParam>");
            init.Initialize(param);

            if (view.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(view);

            var result = view.ShowDialog();

            if (view.DataContext is IClearable clearable)
                clearable.Clear();

            return result;
        }*/

        /// <inheritdoc />
        public TResult ShowDialog<TWindow, TResult>(Func<TWindow, TResult> selector)
            where TWindow : Window, IViewWithResult<TResult>
        {
            TWindow view = CreateWindow<TWindow>();

            if (view.DataContext is IInitializable initializable)
                initializable.Initialize();

            SetOwner(view);
            bool dialogResult = view.ShowDialog(this.GetShellWindow()).IsCompletedSuccessfully == true;

            return dialogResult ? view.Result : default(TResult);
        }

        /// <inheritdoc />
        public async Task<TResult> ShowDialogAsync<TWindow, TResult, TParam>(TParam param, bool canMinimize = false)
            where TWindow : Window, IViewWithResult<TResult>
        {
            TWindow view = CreateWindow<TWindow>(canMinimize);

            if (view.DataContext is not BindableBase viewModel) // ObservableViewModelBase
                throw new InvalidOperationException("ViewModel must implement BindableBase");

            if (view.DataContext is not IInitializable<TParam> init)
                throw new InvalidOperationException("ViewModel must implement IInitializable<TParam>");

            if (view.DataContext is not IResult<TResult> dialogResult)
            {
                throw new InvalidOperationException("ViewModel must implement IResult<TResult>");
            }

            if (view.DataContext is IInitializable initializable)
            {
                initializable.Initialize();
            }

            init.Initialize(param);

            Window ownerWindow = SetOwner(view);

            TResult result = await view.ShowDialog<TResult>(ownerWindow);

            return result != null ? result : default(TResult);
        }


        public TResult ShowDialogSync<TWindow, TResult, TParam>(TParam param, bool canMinimize = false)
            where TWindow : Window, IViewWithResult<TResult>
        {
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                Window window = this.GetShellWindow();
                TWindow view = CreateWindow<TWindow>();

                Task<TResult> task = view.ShowDialog<TResult>(window);
                task.ContinueWith(t => { source.Cancel(); });
                //Dispatcher.UIThread.Post(ShowDialog<>());
                return view.Result;
            }
        }

        /// <inheritdoc />
        public TView CreateView<TView>() where TView : Control
        {
            return _container.Resolve<TView>();
        }

        /// <inheritdoc />
        public Window GetShellWindow()
        {
            //return _shell ?? (_shell = CreateWindow<TShell>(true));
            if (_shell == null)
            {
                _shell = CreateWindow<TShellView>(true);
                if (_shell.DataContext is IInitializable initializable)
                    initializable.Initialize();
            }

            return _shell;
        }

        /// <inheritdoc />
        public void CloseShell()
        {
            if (Application.Current != null &&
                Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
                    desktopStyleApplicationLifetime)
            {
                desktopStyleApplicationLifetime.Shutdown();
            }
        }

        /// <inheritdoc />
        public void RemoveCloseWindowBehavior<TWindow>(TWindow window) where TWindow : Window
        {
            /*var behaviors = Interaction.GetBehaviors(window);
            var closeBehavior = behaviors.OfType<CloseWindowBehavior>().FirstOrDefault();
            if (closeBehavior != null)
                Interaction.GetBehaviors(window).Remove(closeBehavior);*/
        }

        private TWindow CreateWindow<TWindow>(bool canMinimize = false) where TWindow : Window
        {
            TWindow window = CreateView<TWindow>();

            /*if (!canMinimize)
               Interaction.GetBehaviors(window).Add(new DisableMinimizeBehavior());*/

            // Добавление механизма масштабирования к создаваемому окну
            /*if (window.Content is Control control)
            {
                var scaleTransform = new ScaleTransform();
                var scaleValueBinding = new Binding
                {
                    Source = _presentationParameters,
                    Path = new PropertyPath(nameof(_presentationParameters.ScaleFactor)),
                    Mode = BindingMode.OneWay
                };
                BindingOperations.SetBinding(scaleTransform, ScaleTransform.ScaleXProperty, scaleValueBinding);
                BindingOperations.SetBinding(scaleTransform, ScaleTransform.ScaleYProperty, scaleValueBinding);
                control.LayoutTransform = scaleTransform;
            }*/
            return window;
        }

        /// <summary>
        /// Установить Shell в качестве родительского окна
        /// </summary>
        /// <typeparam name="TWindow"></typeparam>
        /// <param name="window"></param>
        private Window SetOwner<TWindow>(TWindow window) where TWindow : Window
        {
            Window ownerWindow = FindActiveOwner(window);

            if (ownerWindow == null)
            {
                Window shell = CreateWindow<TShellView>(true);

                // Если приложение свернуто, то нужно найти и активировать верхнее окно.
                ownerWindow = FindTopMostWindow(shell) ?? shell;
                ownerWindow.Activate();
            }

            return ownerWindow;
        }

        /// <summary>
        /// Рекурсивный поиск активного окна и установка его как Owner.
        /// </summary>
        /// <param name="possibleOwner"></param>
        /// <param name="childWindow"></param>
        /// <returns></returns>
        private bool FindActiveOwner(Window possibleOwner, Window childWindow)
        {
            if (possibleOwner != null)
            {
                if (possibleOwner.IsActive) //Если possibleOwner активный
                {
                    if (possibleOwner.IsVisible)
                    {
                        //childWindow.Owner = possibleOwner;
                        return true;
                    }
                }

                //Поиск активного дочернего окна.
                foreach (Window ownedWindow in possibleOwner.OwnedWindows)
                {
                    if (FindActiveOwner(ownedWindow, childWindow))
                        return true;
                }

                // Если нет активных дочерних окон, то пусть будет главное окно.
                if (possibleOwner.IsVisible)
                {
                    //childWindow.Owner = possibleOwner;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Рекурсивный поиск активного окна и установка его как Owner.
        /// </summary>
        /// <param name="childWindow"></param>
        /// <returns></returns>
        private Window FindActiveOwner(Window childWindow)
        {
            if (Application.Current == null ||
                Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime applicationLifetime)
            {
                return null;
            }

            return applicationLifetime.MainWindow ??
                   applicationLifetime.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive && !w.Equals(childWindow));
        }

        /// <summary>
        /// Нужно найти и активировать верхнее окно.
        /// </summary>
        /// <param name="bottomWindow"></param>
        /// <returns></returns>
        private Window FindTopMostWindow(Window bottomWindow)
        {
            Window childWindow = null;

            if (Application.Current != null &&
                Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime)
            {
                childWindow = applicationLifetime.Windows.OfType<Window>()
                    .FirstOrDefault(w => w.Owner != null && w.Owner.Equals(bottomWindow));
            }

            return childWindow == null ? bottomWindow : FindTopMostWindow(childWindow);
        }

        private readonly IContainer _container;
        private readonly IPresentationParameters _presentationParameters;
        private TShellView _shell;
    }
}