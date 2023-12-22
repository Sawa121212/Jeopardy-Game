using TopicDb.Domain;

namespace TopicsDB.Infrastructure.Managers
{
    public interface ITopicDbManager
    {
        TopicDbContext DbContext { get; }
        bool IsConnected();
    }
}