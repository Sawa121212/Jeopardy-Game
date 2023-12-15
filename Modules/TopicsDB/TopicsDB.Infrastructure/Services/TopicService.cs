using System.Collections.Generic;
using System.Linq;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Services
{
    public class TopicService : ITopicService
    {
        private readonly TopicDbContext _context;

        public TopicService(TopicDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            _context.SaveChanges();
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
            if (_context.Topics.Contains(topic))
            {
                DeleteTopic(topic.Id);
            }
        }

        public void DeleteTopic(int topicId)
        {
            Topic topic = _context.Topics.Find(topicId);
            if (topic == null)
            {
                return;
            }

            _context.Topics.Remove(topic);
            _context.SaveChanges();
        }

        /// <inheritdoc />
        public Topic GetTopicById(int topicId)
        {
            return _context.Topics.Find(topicId);
        }

        /// <inheritdoc />
        public List<Topic> GetAllTopics()
        {
            return _context.Topics.ToList();
        }

        private void UpdateTopic(int topicId, Topic updatedTopic)
        {
            Topic topic = _context.Topics.Find(topicId);
            if (topic == null)
            {
                return;
            }

            topic.Name = updatedTopic.Name;
            // обновление других свойств
            _context.SaveChanges();
        }
    }
}