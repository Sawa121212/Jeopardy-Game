using TopicDb.Domain;
using TopicsDB.Infrastructure.Interfaces.Managers;

namespace TopicsDB.Infrastructure.Managers
{
    public class TopicDbManager : ITopicDbManager
    {
        public TopicDbManager()
        {
            _topicDbContext = new TopicDbContext();
        }
        
        /// <inheritdoc />
        public TopicDbContext DbContext => _topicDbContext;

        /// <inheritdoc />
        public bool IsConnected()
        {
            return _topicDbContext.Database.CanConnect();
        }

        private readonly TopicDbContext _topicDbContext;
    }
}