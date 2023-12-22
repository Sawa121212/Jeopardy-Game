using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Entities;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using Common.Extensions;
using DynamicData;
using Infrastructure.Interfaces.Managers;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Infrastructure.Module.Views
{
    public class SettingsViewModel : NavigationViewModelBase
    {
        public SettingsViewModel(IRegionManager regionManager, ISettingsViewManager settingsManager) : base(regionManager)
        {
            _regionManager = regionManager;
            _settingsManager = settingsManager;

            MenuElementChangedCommand = new DelegateCommand<GroupedElement>(SelectionChanged);
            OkCommand = new DelegateCommand(OnShowManinView);

            ExtraMenuElements = new ObservableCollection<NodeElement>();

            CreateGroups();

            if (MenuElements.Any())
            {
                SelectionChanged(MenuElements.First());
            }
        }

        /// <summary>
        /// Элементы в меню
        /// </summary>
        public ObservableCollection<GroupedElement> MenuElements
        {
            get => _menuElements;
            set => this.RaiseAndSetIfChanged(ref _menuElements, value);
        }

        /// <summary>
        /// Список настроек у Элемента из меню
        /// </summary>
        public ObservableCollection<NodeElement> ExtraMenuElements
        {
            get => _extraMenuElements;
            set => this.RaiseAndSetIfChanged(ref _extraMenuElements, value);
        }

        /// <summary>
        /// Выбранный вью
        /// </summary>
        public string SelectedViewName
        {
            get => _selectedViewName;
            set => this.RaiseAndSetIfChanged(ref _selectedViewName, value);
        }

        /// <summary>
        /// Выбранный элемент из меню
        /// </summary>
        public GroupedElement SelectedGroup
        {
            get => _selectedGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedGroup, value);
        }

        /// <summary>
        /// Выбранная настройка со списка у Элемента из меню
        /// </summary>
        public object SelectedGroupNode
        {
            get => _selectedNode;
            set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
        }

        public ICommand MenuElementChangedCommand { get; }
        public ICommand OkCommand { get; } // ToDo Remove

        /// <summary>
        /// Меняем выбранный элемент
        /// </summary>
        /// <param name="menuElement"></param>
        private void SelectionChanged(GroupedElement menuElement)
        {
            if (menuElement == null)
                return;

            SelectedGroupNode = null;
            SelectedViewName = null;

            string viewName = _settingsManager.GetView(menuElement);

            if (!viewName.IsNullOrEmpty())
            {
                SelectedViewName = viewName;
                bool isRegionContains = _regionManager.Regions.ContainsRegionWithName(RegionNameService.SettingsContentRegionName);
                if (isRegionContains)
                {
                    _regionManager.RequestNavigate(RegionNameService.SettingsContentRegionName, SelectedViewName);
                }
                else
                {
                    _regionManager.RegisterViewWithRegion(RegionNameService.SettingsContentRegionName, SelectedViewName);
                }
            }
            else
            {
                //Группа в меню
                SelectedGroup = menuElement;

                ExtraMenuElements.Clear();

                if (menuElement.Children.Any())
                {
                    if (menuElement.Children.Count == 1)
                    {
                        SelectionChanged(menuElement.Children.First() as GroupedElement);
                        return;
                    }

                    ExtraMenuElements.Add(menuElement.Children);
                }
            }
        }

        /// <summary>
        /// Создать группы
        /// </summary>
        private void CreateGroups()
        {
            Dictionary<string, GroupedElement> groups = new();

            foreach (GroupedElement menuElement in _settingsManager.GetMenuElements())
            {
                if (string.IsNullOrEmpty(menuElement.GroupName))
                    menuElement.GroupName = DefaultGroupName; //Зададим группу по умолчанию.

                if (!groups.ContainsKey(menuElement.GroupName))
                    groups.Add(menuElement.GroupName, new GroupedElement(menuElement.GroupName)); //Создадим группу

                GroupedElement groupElement = groups.GetValueOrDefault(menuElement.GroupName);
                groupElement?.AddChild(menuElement); //Добавим элемент в группу с нужным названием.
            }

            MenuElements = new ObservableCollection<GroupedElement>(groups.Values);
        }

        private void OnShowManinView()
        {
            _regionManager.RequestNavigate(RegionNameService.ShellRegionName, "MainView");
        }

        private const string DefaultGroupName = "Общие";
        private readonly IRegionManager _regionManager;
        private readonly ISettingsViewManager _settingsManager;
        private ObservableCollection<GroupedElement> _menuElements;
        private string _selectedViewName;
        private GroupedElement _selectedGroup;
        private object _selectedNode;
        private ObservableCollection<NodeElement> _extraMenuElements;
        private string _settingsContentRegionName;
    }
}