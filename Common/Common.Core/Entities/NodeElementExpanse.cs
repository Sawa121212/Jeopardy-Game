using Avalonia.Controls;
using ReactiveUI;

namespace Common.Core.Entities
{
    public class NodeElementExpanse : NodeElement, ISelectable
    {
        private string _icon;
        private object _parameter;
        private bool _isExpanded;
        private bool _isSelected;

        public NodeElementExpanse(string name, NodeElement parent = null)
            : base(name, parent)
        {
        }

        public string Icon
        {
            get => _icon;
            set => this.RaiseAndSetIfChanged(ref _icon, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }

        public object Parameter
        {
            get => _parameter;
            set => this.RaiseAndSetIfChanged(ref _parameter, value);
        }

        /// <summary>
        /// Флаг о выборе элемента.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}