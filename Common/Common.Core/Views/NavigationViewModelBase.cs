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
        /// ��� �������� � �������������. ���������� ����� ���������� ���������
        /// </summary>
        /// <param name="navigationContext">��������� ���������</param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        /// <summary>
        /// ��� �������� �� ���. ���������� �� ������ ���������
        /// </summary>
        /// <param name="navigationContext">��������� ���������</param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// ��������� ������������� (�������������) ������������� ��� ������ ������������� �������,
        /// ����� �� ��� ���������� ������ ���������
        /// </summary>
        /// <param name="navigationContext">��������� ���������</param>
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