using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Views
{
    public partial class TopicListViewModel : BindableBase
    {
        private ObservableCollection<Topic> _topics;

        public TopicListViewModel(ITopicService topicService)
        {
            _topicService = topicService;
            Topics = new ObservableCollection<Topic>(_topicService.GetAllTopics());

            EditTopicCommand = new DelegateCommand(OnEditTopic);
            AddCommand = new DelegateCommand(OnAdd);
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            FindTopicsCommand = new DelegateCommand(OnFindTopics);
            ClearFoundElementsCommand = new DelegateCommand(OnClearFoundElements);
        }

        private void OnEditTopic()
        {
        }

        private void UpdateCustomersInformation()
        {
            Topics.Clear();
            foreach (Topic customer in _topicService.GetAllTopics())
            {
                Topics.Add(customer);
            }
        }

        private async void OnAdd()
        {
            /*var result = await _dialogManager.ShowDialog<AddKeyView, License, Customer>(customer);
            if (result != null)
            {
                _dbManager.AddLicenseKey(customer, result);
                await _confirmationService.ShowInfo("Успешно", "Лицензионный ключ добавлен");
                UpdateCustomersInformation();
            }*/
        }

        private async void OnDelete(object element)
        {
            /*var infoResult = await _confirmationService.ShowInfo("Подтвердите удаление",
                "Вы действительно хотите удалить выбранного пользователя?", ConfirmationResult.Yes | ConfirmationResult.No);
            if (infoResult == ConfirmationResult.Yes)
            {
                result = Topics.Remove(customer);
                _dbManager.RemoveCustomer(customer);
                await _confirmationService.ShowInfo("Успешно", "Пользователь успешно удален");
                UpdateCustomersInformation();
                _eventAggregator.GetEvent<NavigateViewChangedEvent>().Publish();
            }*/


            /*if (result is false)
            {
                await _confirmationService.ShowError("Error", "Ошибка при удалении");
            }*/
        }


        public ObservableCollection<Topic> Topics
        {
            get => _topics;
            set => SetProperty(ref _topics, value);
        }

        public ICommand EditTopicCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly ITopicService _topicService;
    }
}