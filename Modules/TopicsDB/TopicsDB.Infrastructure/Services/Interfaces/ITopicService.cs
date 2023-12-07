using System.Collections.Generic;
using TopicDb.Domain.Models;

namespace TopicsDB.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// для управления темами
    /// </summary>
    public interface ITopicService
    {
        /// <summary>
        /// создание новой темы
        /// </summary>
        /// <param name="topic"></param>
        public void CreateTopic(Topic topic);

        /// <summary>
        /// редактирование существующей темы
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="updatedTopic"></param>
        public void UpdateTopic(int topicId, Topic updatedTopic);

        /// <summary>
        /// удаление темы
        /// </summary>
        /// <param name="topicId"></param>
        public void DeleteTopic(int topicId);

        /// <summary>
        /// получение темы по идентификатору
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public Topic GetTopicById(int topicId);

        /// <summary>
        /// получение всех тем
        /// </summary>
        /// <returns></returns>
        public List<Topic> GetAllTopics();
    }
}