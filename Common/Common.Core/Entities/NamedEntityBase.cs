using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace Common.Core.Entities
{
    /// <summary>
    /// Сущность c уникальным идентификатором и именем.
    /// </summary>
    /// <typeparam name="TId">Идентификатора.</typeparam>
    /// <typeparam name="TName">Имя.</typeparam>
    public class NamedEntityBase<TId, TName> : EntityBase<TId> where TId : struct, IEquatable<TId>
    {
        private bool _created;

        // ReSharper disable once InconsistentNaming
        internal const string NAME = "name";

        private TName _name;

        /// <summary>
        /// Конструктор сущности, обладающей уникальным идентификатором и именем.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <param name="name">Имя сущности.</param>
        protected NamedEntityBase(TId id, TName name) : base(id)
        {
            Name = name;
            /* ValidationRules.Add(new RelayValidationRule(nameof(Name), "Задайте имя",
                _ => Name != null && !string.IsNullOrEmpty(Name.ToString())));*/
            _created = true;
        }

        /// <summary>
        /// Наименование сущности.
        /// </summary>
        [Category("Общая часть")]
        // ToDo: [PropertyOrder(0)]
        [Browsable(true)]
        [ReadOnly(false)]
        [MaxLength(50)]
        [DisplayName("Название")]
        public TName Name
        {
            get => _name;
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);

                if (_created)
                {
                    NameChanged();
                }
            }
        }

        /// <summary>
        /// Вызывается при изменении имени
        /// </summary>
        public virtual void NameChanged()
        {
        }
    }
}