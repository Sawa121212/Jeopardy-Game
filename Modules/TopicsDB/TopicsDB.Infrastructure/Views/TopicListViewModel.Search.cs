using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using TopicDb.Domain.Models;

namespace TopicsDB.Infrastructure.Views
{
    public partial class TopicListViewModel
    {
        private string _filterText;

        private void OnFindTopics()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                return;
            }

            string searchText = FilterText.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            List<Topic> allTopics = _topicService.GetAllTopics();
            if (!allTopics.Any())
            {
                return;
            }

            List<Topic> foundedElements = new(allTopics.Where(o => o.Name != null && o.Name.ToLower().Contains(searchText)));
            if (!foundedElements.Any())
            {
                return;
            }

            Topics.Clear();
            Topics.AddRange(foundedElements);
        }

        private void OnClearFoundElements()
        {
            FilterText = string.Empty;
            Topics = new ObservableCollection<Topic>(_topicService.GetAllTopics());
        }

        public string FilterText
        {
            get => _filterText;
            set => this.RaiseAndSetIfChanged(ref _filterText, value);
        }

        public ICommand FindTopicsCommand { get; }
        public ICommand ClearFoundElementsCommand { get; }
    }
}