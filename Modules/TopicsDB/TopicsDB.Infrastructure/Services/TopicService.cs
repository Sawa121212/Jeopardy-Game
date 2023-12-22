using System.Collections.Generic;
using System.Linq;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Managers;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicDbManager _topicDbManager;
        private readonly TopicDbContext _dbContext;

        public TopicService(ITopicDbManager topicDbManager)
        {
            _topicDbManager = topicDbManager;
            _dbContext = _topicDbManager.DbContext;
        }

        /// <inheritdoc />
        public void CreateTopic(Topic topic)
        {
            _dbContext.Topics.Add(topic);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public void UpdateTopic(Topic updatedTopic)
        {
            if (updatedTopic is null)
            {
                return;
            }

            UpdateTopic(updatedTopic.Id, updatedTopic);
        }

        /// <inheritdoc />
        public void DeleteTopic(Topic topic)
        {
            if (_dbContext.Topics.Contains(topic))
            {
                DeleteTopic(topic.Id);
            }
        }

        public void DeleteTopic(int topicId)
        {
            Topic topic = _dbContext.Topics.Find(topicId);
            if (topic == null)
            {
                return;
            }

            _dbContext.Topics.Remove(topic);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public Topic GetTopicById(int topicId)
        {
            return _dbContext.Topics.Find(topicId);
        }

        /// <inheritdoc />
        public List<Topic> GetAllTopics()
        {
            return _dbContext.Topics.ToList();
        }

        private void UpdateTopic(int topicId, Topic updatedTopic)
        {
            Topic topic = _dbContext.Topics.Find(topicId);
            if (topic == null)
            {
                return;
            }

            topic.Name = updatedTopic.Name;
            // обновление других свойств
            _dbContext.SaveChanges();
        }
    }
}