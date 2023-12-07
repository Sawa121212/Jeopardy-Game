using System.Collections.Generic;
using System.Linq;
using TopicDb.Domain;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly TopicDbContext _context;

        public QuestionService(TopicDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void CreateQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        /// <inheritdoc />
        public void UpdateQuestion(int questionId, Question updatedQuestion)
        {
            Question question = _context.Questions.Find(questionId);
            if (question != null)
            {
                question.Text = updatedQuestion.Text;
                // обновление других свойств
                _context.SaveChanges();
            }
        }

        /// <inheritdoc />
        public void DeleteQuestion(int questionId)
        {
            Question question = _context.Questions.Find(questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }

        /// <inheritdoc />
        public Question GetQuestionById(int questionId)
        {
            return _context.Questions.Find(questionId);
        }

        /// <inheritdoc />
        public List<Question> GetAllQuestionsByTopic(int topicId)
        {
            return _context.Questions.Where(q => q.TopicId == topicId).ToList();
        }
    }
}