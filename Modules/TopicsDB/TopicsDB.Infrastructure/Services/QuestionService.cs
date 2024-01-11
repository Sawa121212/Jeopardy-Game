using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain.Helpers;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Managers;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        public QuestionService(ITopicDbManager topicDbManager, ITopicService topicService)
        {
            _topicDbManager = topicDbManager;
            _dbContext = _topicDbManager.DbContext;
            _topicService = topicService;
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
        public List<Question> GetAllQuestionsByTopicId(int topicId)
        {
            return _dbContext.Questions.Where(q => q.TopicId == topicId).ToList();
        }

        /// <inheritdoc />
        public Question GetRandomQuestionFromTopicByPrice(int topicId, int price)
        {
            Topic topic = _topicService.GetTopicById(topicId);
            List<Question> questionsWithPrice = topic.Questions
                .Where(q => q.Price == price)
                .ToList();

            if (questionsWithPrice.Count == 0)
            {
                throw new ArgumentException($"No questions found for topic '{topic.Name}' with price {price}.");
            }

            int randomIndex = RandomGenerator.GetRandom().Next(0, questionsWithPrice.Count);
            return questionsWithPrice[randomIndex];
        }

        private readonly ITopicDbManager _topicDbManager;
        private readonly ITopicService _topicService;
        private readonly TopicDbContext _dbContext;
    }
}