﻿using System.Collections.Generic;
using Avalonia.Controls;
using Common.Core.Entities;
using Common.Extensions;
using Common.Extensions.Collections;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;

namespace Infrastructure.Environment.Managers
{
    /// <inheritdoc />
    public class SettingsViewManager : ISettingsViewManager
    {
        private readonly IContainerProvider _container;
        private readonly IDictionary<GroupedElement, string> _viewDictionary;

        public SettingsViewManager(IContainerProvider container)
        {
            _container = container;
            _viewDictionary = new Dictionary<GroupedElement, string>();
        }

        /// <inheritdoc />
        public void AddView<TView>(string name, string groupName = null) where TView : Control
        {
            string viewName = typeof(TView).Name;
            if (viewName.IsNullOrEmpty())
            {
                return;
            }

            GroupedElement menuElement = new(name, groupName);
            _viewDictionary.Add(menuElement, viewName);
        }
        
        /// <inheritdoc />
        public string GetView(GroupedElement menuElement)
        {
            return menuElement != null ? _viewDictionary.GetValueOrDefault(menuElement) : null;
        }

        /// <inheritdoc />
        public IEnumerable<GroupedElement> GetMenuElements()
        {
            return new List<GroupedElement>(_viewDictionary.Keys);
        }
    }
}