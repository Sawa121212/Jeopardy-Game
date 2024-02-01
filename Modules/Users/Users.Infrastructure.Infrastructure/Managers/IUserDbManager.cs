using Users.Domain;

namespace Users.Infrastructure.Interfaces.Managers
{
    public interface IUserDbManager
    {
        UserDbContext DbContext { get; }
    }
}