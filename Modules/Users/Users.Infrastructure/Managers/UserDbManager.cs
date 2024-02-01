using Users.Domain;
using Users.Infrastructure.Interfaces.Managers;

namespace Users.Infrastructure.Managers
{
    public class UserDbManager : IUserDbManager
    {
        public UserDbManager()
        {
            _topicDbContext = new UserDbContext();
        }

        /// <inheritdoc />
        public UserDbContext DbContext => _topicDbContext;

        private readonly UserDbContext _topicDbContext;
    }
}