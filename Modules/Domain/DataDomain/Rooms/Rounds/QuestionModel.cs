using ReactiveUI;
using TopicDb.Domain.Models;

namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public class QuestionModel : ReactiveObject
    {
        public QuestionModel(Question? question, int point)
        {
            Id = question.Id;
            TopicId = question.TopicId;
            Text = question.Text;
            Answer = question.CorrectAnswer;
            Price = point;

            //Picture = question.Text;
        }

        public int Id
        {
            get => _id;
            init => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public int TopicId
        {
            get => _topicId;
            init => this.RaiseAndSetIfChanged(ref _topicId, value);
        }

        public string Text
        {
            get => _text;
            init => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public object Picture
        {
            get => _picture;
            init => this.RaiseAndSetIfChanged(ref _picture, value);
        }

        public int Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value); // игроки могут делать ставки
        }

        /// <summary>
        /// Правильный ответ
        /// </summary>
        public string Answer
        {
            get => _answer;
            init => this.RaiseAndSetIfChanged(ref _answer, value);
        }

        /// <summary>
        /// Вопрос был задан
        /// </summary>
        public bool IsAsked
        {
            get => _isAsked;
            set => this.RaiseAndSetIfChanged(ref _isAsked, value);
        }

        private string _text;
        private object _picture;
        private int _price;
        private string _answer;
        private bool _isAsked;
        private int _id;
        private int _topicId;
    }
}