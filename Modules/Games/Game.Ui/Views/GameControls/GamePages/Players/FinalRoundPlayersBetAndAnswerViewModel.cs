using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Domain.Data;
using Game.Ui.Models;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.GamePages.Players
{
    public class FinalRoundPlayersBetAndAnswerViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public FinalRoundPlayersBetAndAnswerViewModel(IRegionManager regionManager) : base(regionManager)
        {
            ShowNextPlayerCommand = new DelegateCommand(OnShowNextPlayer);
            AnsweredQuestionCommand = new DelegateCommand<object>(OnAnsweredQuestionCommand);
            ContinueGameCommand = new DelegateCommand(OnContinueGame);
        }

        /// <summary>
        /// Отображаемая тема
        /// </summary>
        public ObservableCollection<PlayerBetModel>? PlayerBetModels
        {
            get => _playerBetModels;
            set => this.RaiseAndSetIfChanged(ref _playerBetModels, value);
        }

        /// <summary>
        /// Отображаемая тема
        /// </summary>
        public PlayerBetModel? ShowedPlayerBetModel
        {
            get => _showedPlayerBetModel;
            private set => this.RaiseAndSetIfChanged(ref _showedPlayerBetModel, value);
        }

        public QuestionModel? DisplayedQuestion
        {
            get => _displayedQuestion;
            set => this.RaiseAndSetIfChanged(ref _displayedQuestion, value);
        }

        /// <summary>
        /// Все темы показаны
        /// </summary>
        public bool IsShowedAllTopic
        {
            get => _isShowedAllPlayers;
            private set => this.RaiseAndSetIfChanged(ref _isShowedAllPlayers, value);
        }

        public ICommand ShowNextPlayerCommand { get; }
        public ICommand ContinueGameCommand { get; }
        public ICommand AnsweredQuestionCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            _playerBetModels = default;
            ShowedPlayerBetModel = null;

            IList<PlayerBetModel> playerBetModelsParameter = navigationContext.Parameters
                .GetValue<IList<PlayerBetModel>>(NavigationParameterService.InitializeParameter);

            if (playerBetModelsParameter != null && playerBetModelsParameter.Any())
            {
                PlayerBetModels = new ObservableCollection<PlayerBetModel>(playerBetModelsParameter);
                OnShowNextPlayer();
            }

            QuestionModel questionModel = navigationContext.Parameters
                .GetValue<QuestionModel>(NavigationParameterService.InitializeSecondParameter);

            if (questionModel != null)
            {
                DisplayedQuestion = questionModel;
            }

            IsShowedAllTopic = false;
        }

        /// <summary>
        /// Указать, правильно ли ответил игрок
        /// </summary>
        /// <param name="value"></param>
        private void OnAnsweredQuestionCommand(object value)
        {
            if (value is not bool isCorrectAnswer)
            {
                return;
            }

            if (ShowedPlayerBetModel != null)
            {
                ShowedPlayerBetModel.IsCorrectAnswer = isCorrectAnswer;
            }
        }

        /// <summary>
        /// Отобразить следующего игрока
        /// </summary>
        private void OnShowNextPlayer()
        {
            if (_playerBetModels == null || !_playerBetModels.Any())
            {
                return;
            }

            if (ShowedPlayerBetModel is null)
            {
                ShowedPlayerBetModel = _playerBetModels.First();

                return;
            }

            int nextIndex = _playerBetModels.IndexOf(_showedPlayerBetModel) + 1;

            if (nextIndex >= _playerBetModels.Count)
            {
                return;
            }

            ShowedPlayerBetModel = _playerBetModels[nextIndex];

            if (nextIndex + 1 == _playerBetModels.Count)
            {
                IsShowedAllTopic = true;
            }
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void OnContinueGame()
        {
            if (DisplayedQuestion != null)
            {
                DisplayedQuestion.IsAsked = true;
            }

            MoveBackCommand.Execute(default);
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, GameStatusEnum.SetPlayerBets);
        }

        private PlayerBetModel? _showedPlayerBetModel;
        private ObservableCollection<PlayerBetModel>? _playerBetModels;
        private bool _isShowedAllPlayers;
        private QuestionModel? _displayedQuestion;
    }
}