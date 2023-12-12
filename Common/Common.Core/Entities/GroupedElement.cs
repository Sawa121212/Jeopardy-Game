using System;
using ReactiveUI;

namespace Common.Core.Entities
{
    /// <summary>
    /// Элемент дерева с названием группы.
    /// </summary>
    public class GroupedElement : NodeElement
    {
        private string _groupName;

        public GroupedElement(Guid guid, string name, string groupName = null, NodeElementExpanse parent = null)
            : base(guid, name, parent)
        {
            GroupName = groupName;
        }

        public GroupedElement(string name, string groupName = null, NodeElementExpanse parent = null)
            : base(name, parent)
        {
            GroupName = groupName;
        }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string GroupName
        {
            get => _groupName;
            set => this.RaiseAndSetIfChanged(ref _groupName, value);
        }
    }
}