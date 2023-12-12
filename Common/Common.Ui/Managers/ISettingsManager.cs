using System.Collections.Generic;
using Avalonia.Controls;
using Common.Core.Entities;

namespace Common.Ui.Managers
{
    /// <summary>
    /// Менеджер отвечает за все что связано с настройками.
    /// </summary>
    public interface ISettingsViewManager
    {
        /// <summary>
        /// Добавить вьюху настроек
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        void AddView<TView>(string name, string groupName = null) where TView : Control;

        /// <summary>
        /// Получить въюху с настройками
        /// </summary>
        /// <param name="menuElement">Соответствующий названию настроек элемент.</param>
        /// <returns>Вид с настройками.</returns>
        Control GetView(GroupedElement menuElement);

        /// <summary>
        /// Получить список сущностей.
        /// </summary>
        /// <returns></returns>
        IEnumerable<GroupedElement> GetMenuElements();
    }
}