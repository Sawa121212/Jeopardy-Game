using System.Collections.Generic;
using TopicDb.Domain.Models;

namespace TopicsDB.Infrastructure.Interfaces.Services
{
    /// <summary>
    /// для управления темами
    /// </summary>
    public interface ITopicService
    {
        /// <summary>
        /// Создание новой темы
        /// </summary>
        /// <param name="topic"></param>
        public void CreateTopic(Topic topic);

        /// <summary>
        /// Редактирование существующей темы
        /// </summary>
        /// <param name="updatedTopic"></param>
        public void UpdateTopic(Topic updatedTopic);

        /// <summary>
        /// Удаление темы
        /// </summary>
        /// <param name="topic">Тема</param>
        public void DeleteTopic(Topic topic);

        /// <summary>
        /// Получение темы по идентификатору
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public Topic GetTopicById(int topicId);

        /// <summary>
        /// Получение всех тем
        /// </summary>
        /// <returns></returns>
        public List<Topic> GetAllTopics();

        /// <summary>
        /// Получить количество всех тем
        /// </summary>
        /// <returns></returns>
        int GetAllTopicsCount();
    }
}