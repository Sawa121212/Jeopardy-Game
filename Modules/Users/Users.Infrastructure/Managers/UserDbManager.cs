using Users.Domain;
using Users.Infrastructure.Interfaces.Managers;

namespace Users.Infrastructure.Managers
{
    public class UserDbManager : IUserDbManager
    {
        public UserDbManager(UserDbContext userDbContext)
        {
            _topicDbContext = userDbContext;
        }

        /// <inheritdoc />
        public UserDbContext DbContext => _topicDbContext;

        private UserDbContext _topicDbContext;
    }
}