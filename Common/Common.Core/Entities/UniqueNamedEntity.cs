using System;

namespace Common.Core.Entities
{
    /// <summary>
    /// Сущность c уникальным идентификатором и именем.
    /// </summary>
    public class UniqueNamedEntity : NamedEntityBase<Guid, string>
    {
        /// <inheritdoc />
        public UniqueNamedEntity(Guid guid, string name) : base(guid, name)
        {
        }
    }
}