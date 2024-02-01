using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Managers;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace TopicsDB.Infrastructure.Services
{
    public class TopicService : ITopicService
    {
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
            return _dbContext.Topics
                .Include(t => t.Questions)
                .ToList();
        }

        /// <inheritdoc />
        public int GetAllTopicsCount() => _dbContext.Topics.Count();

        private void UpdateTopic(int topicId, Topic updatedTopic)
        {
            Topic topic = _dbContext.Topics
                .Find(topicId);
            if (topic == null)
            {
                return;
            }

            topic.Name = updatedTopic.Name;

            // обновление других свойств
            _dbContext.SaveChanges();
        }

        private readonly ITopicDbManager _topicDbManager;
        private readonly TopicDbContext _dbContext;
    }
}