using System;
using System.ComponentModel;
using ReactiveUI;

namespace Common.Core.Entities
{
    /// <summary>
    /// Сущность c уникальным идентификатором.
    /// </summary>
    /// <typeparam name="TId">Идентификатор.</typeparam>
    public class EntityBase<TId> : ReactiveObject where TId : struct, IEquatable<TId>
    {
        // ReSharper disable once InconsistentNaming
        internal const string ID = "id";

        private readonly int _hash;

        /// <summary>
        /// Конструктор сущности, обладающей уникальным идентификатором.
        /// </summary>
        /// <param name="id"></param>
        protected EntityBase(TId id)
        {
            Uid = id;
            _hash = Uid.GetHashCode();
        }

        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        [Browsable(false)]
        public TId Uid { get; }

        /// <inheritdoc cref="EntityBase{TId}.Equals(object)" />
        public bool Equals(EntityBase<TId> other)
        {
            return Equals((object) other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;
            if (GetType() != obj.GetType())
                return false;

            return Uid.Equals(((EntityBase<TId>) obj).Uid);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _hash;
    }
}