using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ReactiveUI;

namespace Common.Core.Entities
{
    /// <summary>
    /// Уникальная сущность для элемента дерева.
    /// </summary>
    public class NodeElement : UniqueNamedEntity
    {
        private string _description;
        private NodeElement _parent;
        private ObservableCollection<NodeElement> _children;
        private string _prefix;
        private bool _isRequired;
        private string _requirements;

        /// <inheritdoc />
        public NodeElement(Guid guid, string name, NodeElement parent = null) : base(guid, name)
        {
            Parent = parent;
            NamePrefix = string.Empty;
            Children = new ObservableCollection<NodeElement>();
        }

        /// <inheritdoc />
        public NodeElement(string name, NodeElement parent = null) : base(Guid.NewGuid(), name)
        {
            Parent = parent;
            NamePrefix = string.Empty;
            Children = new ObservableCollection<NodeElement>();
        }

        /// <summary>
        /// Корневой элемент.
        /// </summary>
        [Browsable(false)]
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Крайний элемент дерева.
        /// </summary>
        [Browsable(false)]
        public bool IsChildless => Children.Count == 0;

        /*/// <summary>
        /// Детский indexer.
        /// </summary>
        /// <param name="childIndex"></param>
        /// <returns></returns>
        [Browsable(false)]
        public NodeElement this[int childIndex]
        {
            get => Children[childIndex];
            set => Children[childIndex] = value;
        }*/

        /// <summary>
        /// Родительский узел.
        /// </summary>
        [Browsable(false)]
        public NodeElement Parent
        {
            get => _parent;
            set => this.RaiseAndSetIfChanged(ref _parent, value);
        }

        /// <summary>
        /// Префикс к названию.
        /// </summary>
        [Browsable(false)]
        public string NamePrefix
        {
            get => _prefix;
            set => this.RaiseAndSetIfChanged(ref _prefix, value);
        }

        /// <summary>
        /// Обязательный узел. 
        /// </summary>
        [Browsable(false)]
        public bool IsRequired
        {
            get => _isRequired;
            set => this.RaiseAndSetIfChanged(ref _isRequired, value);
        }

        /// <summary>
        /// Дополнительные требования.
        /// </summary>
        [Browsable(false)]
        public string Requirements
        {
            get => _requirements;
            set => this.RaiseAndSetIfChanged(ref _requirements, value);
        }

        /// <summary>
        /// Дочерние элементы.
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<NodeElement> Children
        {
            get => _children;
            private set => this.RaiseAndSetIfChanged(ref _children, value);
        }

        /// <summary>
        /// Описание узла.
        /// </summary>
        [Category("Общая часть")]
        // ToDo [PropertyOrder(1)]
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("Описание")]
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        /// <summary>
        /// Добавить дочерний элемент. 
        /// </summary>
        /// <param name="child">Дочерний элемент.</param>
        /// <param name="index">Индекс элемента в массиве родителя. По умолчанию = -1.</param>
        public virtual void AddChild(NodeElement child, int index = -1)
        {
            if (child != null)
            {
                if (!Children.Contains(child))
                {
                    child.Parent = this;
                    if (index != -1)
                    {
                        if (Children.Count < index)
                        {
                            Children.Add(child);
                        }
                        else Children.Insert(index, child);
                    }
                    else Children.Add(child);
                }
            }
        }

        /// <summary>
        /// Удалить дочерний элемент.
        /// </summary>
        /// <param name="child">Дочерний элемент.</param>
        /// <returns></returns>
        public virtual void RemoveChild(NodeElement child)
        {
            if (child != null)
            {
                if (Children.Remove(child))
                {
                    child.Parent = null;
                }
            }
        }

        /// <summary>
        /// Удаление дочернего элемента по имени.
        /// </summary>
        /// <param name="childName"></param>
        public virtual void RemoveChild(string childName)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                if (Children[i].Name == childName)
                    Children.RemoveAt(i);
            }
        }

        /// <summary>
        /// Получить дочерний элемент по имени.
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        public virtual NodeElement GetChild(string childName)
        {
            return Children.FirstOrDefault(child =>
                child.Name.Equals(childName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}