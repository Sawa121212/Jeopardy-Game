using TopicDb.Domain;

namespace TopicsDB.Infrastructure.Interfaces.Managers
{
    public interface ITopicDbManager
    {
        TopicDbContext DbContext { get; }
        bool IsConnected();
    }
}