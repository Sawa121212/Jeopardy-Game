using System.Collections.Generic;
using System.Linq;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Managers;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ITopicDbManager _topicDbManager;
        private readonly TopicDbContext _dbContext;

        public QuestionService(ITopicDbManager topicDbManager)
        {
            _topicDbManager = topicDbManager;
            _dbContext = _topicDbManager.DbContext;
        }

        /// <inheritdoc />
        public void CreateQuestion(Question question)
        {
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public void UpdateQuestion(int questionId, Question updatedQuestion)
        {
            Question question = _dbContext.Questions.Find(questionId);
            if (question == null)
            {
                return;
            }

            question.ChatId = updatedQuestion.ChatId;
            question.MessageId = updatedQuestion.MessageId;
            question.CorrectAnswer = updatedQuestion.CorrectAnswer;
            question.Price = updatedQuestion.Price;
            // обновление других свойств
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public void DeleteQuestion(Question question)
        {
            if (_dbContext.Questions.Contains(question))
            {
                DeleteQuestion(question.Id);
            }
        }

        public void DeleteQuestion(int questionId)
        {
            Question question = _dbContext.Questions.Find(questionId);
            if (question == null)
            {
                return;
            }

            _dbContext.Questions.Remove(question);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public Question GetQuestionById(int questionId)
        {
            return _dbContext.Questions.Find(questionId);
        }

        /// <inheritdoc />
        public List<Question> GetAllQuestionsByTopic(int topicId)
        {
            return _dbContext.Questions.Where(q => q.TopicId == topicId).ToList();
        }

        /// <summary>
        /// Базовая цена вопроса
        /// </summary>
        private static int BaseQuestionPriceValue => 100;
    }
}