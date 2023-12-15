using System.Collections.Generic;
using TopicDb.Domain.Models;

namespace TopicsDB.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// для управления вопросами
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Создание нового вопроса
        /// </summary>
        /// <param name="question"></param>
        public void CreateQuestion(Question question);

        /// <summary>
        /// Редактирование существующего вопроса
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="updatedQuestion"></param>
        public void UpdateQuestion(int questionId, Question updatedQuestion);

        /// <summary>
        /// Удаление вопроса
        /// </summary>
        /// <param name="questionId"></param>
        public void DeleteQuestion(int questionId);

        /// <summary>
        /// Получение вопроса по идентификатору
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Question GetQuestionById(int questionId);

        /// <summary>
        /// Получение всех вопросов по идентификатору темы
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<Question> GetAllQuestionsByTopic(int topicId);
    }
}