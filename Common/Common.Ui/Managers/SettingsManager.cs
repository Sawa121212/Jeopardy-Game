using System.Collections.Generic;
using Avalonia.Controls;
using Common.Core.Entities;
using Common.Core.Interfaces;
using Common.Extensions.Collections;
using Common.Extensions.Prism;
using Prism.Ioc;

namespace Common.Ui.Managers
{
    /// <inheritdoc />
    public class SettingsManager : ISettingsViewManager
    {
        private readonly IContainerProvider _container;
        private readonly IDictionary<GroupedElement, Control> _viewDictionary;

        public SettingsManager(IContainerProvider container)
        {
            _container = container;
            _viewDictionary = new Dictionary<GroupedElement, Control>();
        }

        /// <inheritdoc />
        public void AddView<TView>(string name, string groupName = null) where TView : Control
        {
            TView view = _container.TryResolveEx<TView>();
            if (view != null)
            {
                GroupedElement menuElement = new GroupedElement(name, groupName);
                _viewDictionary.Add(menuElement, view);
            }
        }

        /// <inheritdoc />
        public Control GetView(GroupedElement menuElement)
        {
            Control view = null;

            if (menuElement != null)
            {
                view = _viewDictionary.GetValueOrDefault(menuElement);
                if (view != null)
                {
                    if (view.DataContext is IInitializable initializable)
                        initializable.Initialize();
                }
            }

            return view;
        }

        /// <inheritdoc />
        public IEnumerable<GroupedElement> GetMenuElements()
        {
            return new List<GroupedElement>(_viewDictionary.Keys);
        }
    }
}