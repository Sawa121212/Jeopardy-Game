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
            Price = point;
            Text = question.Text;
            Answer = question.CorrectAnswer;
            //Picture = question.Text;
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
            init => this.RaiseAndSetIfChanged(ref _price, value);
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
        private readonly int _price;
        private string _answer;
        private bool _isAsked;
    }
}