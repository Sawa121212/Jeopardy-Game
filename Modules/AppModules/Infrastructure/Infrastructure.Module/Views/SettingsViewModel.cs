using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Common.Core.Entities;
using Common.Core.Prism.Regions;
using Common.Core.Signals;
using Common.Core.Views;
using Common.Extensions;
using Common.Ui.Managers;
using DynamicData;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Infrastructure.Module.Views
{
    public class SettingsViewModel : ViewModelBase
    {
        private const string DefaultGroupName = "Общие";
        private readonly ISettingsViewManager _settingsManager;
        private ObservableCollection<GroupedElement> _menuElements;
        private Control _selectedView;
        private GroupedElement _selectedGroup;
        private object _selectedNode;
        private ObservableCollection<NodeElement> _extraMenuElements;
        private string _settingsContentRegionName;
        private IRegionManager _region;

        public SettingsViewModel(IRegionManager regionManager, ISettingsViewManager settingsManager)
        {
            _settingsManager = settingsManager;

            Region = regionManager.CreateRegionManager();

            MenuElementChangedCommand = new DelegateCommand<GroupedElement>(SelectionChanged);
            CloseWindowCommand = new DelegateCommand(OnCloseWindow);

            ExtraMenuElements = new ObservableCollection<NodeElement>();
            NeedCloseSignal = new Signal<bool?>();

            CreateGroups();

            if (MenuElements.Any())
            {
                SelectionChanged(MenuElements.First());
            }
        }

        public static bool IsInitialize { get; set; }

        /// <summary>
        /// Меняем выбранный элемент
        /// </summary>
        /// <param name="menuElement"></param>
        private void SelectionChanged(GroupedElement menuElement)
        {
            if (menuElement == null)
                return;

            SelectedGroupNode = null;
            SelectedView = null;

            Control view = _settingsManager.GetView(menuElement);

            if (view != null)
            {
                SelectedView = view;
                string source = view.GetType().Name;
                if (!source.IsNullOrEmpty())
                {
                    Region.RequestNavigate(RegionNameService.SettingsContentRegionName, source);
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
            Dictionary<string, GroupedElement> groups = new Dictionary<string, GroupedElement>();

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

        /// <summary>
        /// Закрытие окна
        /// </summary>
        private void OnCloseWindow()
        {
            NeedCloseSignal.Raise(true);
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
        public Control SelectedView
        {
            get => _selectedView;
            set => this.RaiseAndSetIfChanged(ref _selectedView, value);
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

        /// <summary>
        /// Текущий Регион для окна
        /// </summary>
        public IRegionManager Region
        {
            get => _region;
            set => this.RaiseAndSetIfChanged(ref _region, value);
        }

        public ISignal<bool?> NeedCloseSignal { get; }
        public ICommand MenuElementChangedCommand { get; }
        public ICommand CloseWindowCommand { get; }
    }
}