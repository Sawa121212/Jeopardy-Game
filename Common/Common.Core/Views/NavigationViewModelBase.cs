using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Common.Core.Views
{
    public abstract class NavigationViewModelBase : ReactiveObject, INavigationAware
    {
        private IRegionNavigationJournal _journal;
        protected readonly IRegionManager RegionManager;

        protected NavigationViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
            MoveBackCommand = new DelegateCommand(OnGoBack);
        }

        /// <summary>
        /// При переходе к представлению. Вызывается после завершения навигации
        /// </summary>
        /// <param name="navigationContext">Параметры навигации</param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        /// <summary>
        /// При переходе из вью. Вызывается до начала навигации
        /// </summary>
        /// <param name="navigationContext">Параметры навигации</param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Позволяет существующему (отображаемому) представлению или модели представления указать,
        /// может ли оно обработать запрос навигации
        /// </summary>
        /// <param name="navigationContext">Параметры навигации</param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        private void OnGoBack()
        {
            if (_journal.CanGoBack)
            {
                _journal.GoBack();
                return;
            }

            GoBackOrder();
        }

        protected virtual void GoBackOrder()
        {
        }

        protected virtual async Task GoBackOrderAsync()
        {
        }

        public ICommand MoveBackCommand { get; }
    }
}