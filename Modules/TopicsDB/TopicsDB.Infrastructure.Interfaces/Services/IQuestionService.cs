using System.Collections.Generic;
using TopicDb.Domain.Models;

namespace TopicsDB.Infrastructure.Interfaces.Services
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
        public Question CreateQuestion(Question question);

        /// <summary>
        /// Редактирование существующего вопроса
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="updatedQuestion"></param>
        public void UpdateQuestion(int questionId, Question updatedQuestion);

        /// <summary>
        /// Удаление вопроса
        /// </summary>
        /// <param name="question">Вопроса</param>
        public void DeleteQuestion(Question question);

        /// <summary>
        /// Получение вопроса по идентификатору
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Question GetQuestionById(int questionId);

        /// <summary>
        /// Получение всех
        /// </summary>
        /// <returns></returns>
        List<Question> GetAllQuestions();

        /// <summary>
        /// Получение всех вопросов по идентификатору темы
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<Question> GetAllQuestionsByTopicId(int topicId);

        /// <summary>
        /// Получить случайный вопрос из темы по указанной цене
        /// </summary>
        /// <param name="topicId">ИД темы</param>
        /// <param name="questionBasePrice">Цена вопроса</param>
        /// <returns></returns>
        Question GetRandomQuestionFromTopicByPrice(int topicId, int questionBasePrice);
    }
}