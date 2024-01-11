using System.Collections.Generic;
using ReactiveUI;

namespace DataDomain.Rooms.Rounds
{
    public class TopicModel : ReactiveObject
    {
        public TopicModel()
        {
            Questions = new List<QuestionModel>(5);
        }

        public int Id
        {
            get => _id;
            init => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            init => this.RaiseAndSetIfChanged(ref _name, value);
        }

        /// <summary>
        /// Вопросы
        /// </summary>
        public List<QuestionModel> Questions
        {
            get => _questions;
            set => this.RaiseAndSetIfChanged(ref _questions, value);
        }

        private int _id;
        private string _name;
        private List<QuestionModel> _questions;
    }
}