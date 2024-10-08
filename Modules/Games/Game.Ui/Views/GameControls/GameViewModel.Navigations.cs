using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Prism;
using Confirmation.Module.Enums;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using Game.Domain.Data;
using Game.Ui.Views.GameControls.Pages.GamePages;
using Game.Ui.Views.GameControls.Pages.GamePages.Players;
using Game.Ui.Views.GameControls.Pages.GamePages.QuestionsAndAnswer;
using Game.Ui.Views.GameControls.Pages.GamePages.Rounds;
using Game.Ui.Views.GameControls.Pages.GamePages.Topics;
using Prism.Regions;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand StartGameCommand { get; }

        public ICommand ShowTopicsCarouselCommand { get; }

        public ICommand MoveBackButtonCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // result parameter
            object resultParameter = navigationContext.Parameters[NavigationParameterService.ResultParameter];

            if (resultParameter is GameStatusEnum gameStatus)
            {
                switch (gameStatus)
                {
                    case GameStatusEnum.Continue:
                        return;
                    case GameStatusEnum.ShowRoundLevel:
                        OnShowRoundLevelNameView();

                        return;
                    case GameStatusEnum.ShowCurrentRound:
                        IsShowedTopics = true;
                        OnShowCurrentRoundView();
                        SetPlayerFirstChoosingTopic();

                        return;
                    case GameStatusEnum.GoNextRound:
                        OnGoNextRound();

                        return;
                    case GameStatusEnum.SetPlayerBets:
                        OnSetPlayerBets();

                        return;
                    case GameStatusEnum.EndGame_ShowWinner:
                        OnGoNextRound();

                        return;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Initialize parameter
            /*object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            string? value = parameter?.ToString();

            if (string.IsNullOrEmpty(value))
            {
                return;
            }*/

            ClearAllParameters();

            Rounds = new ObservableCollection<RoundModel?>();

            _game = _gameManager.GetGame();

            if (_game is null)
            {
                Message = "Ошибка. Игра не найдена";

                return;
            }

            if (_game?.Rounds is null || _game.Rounds.Count == 0)
            {
                Message = "Ошибка. Не удалось собрать раунд";

                return;
            }

            Rounds = new ObservableCollection<RoundModel?>(_game.Rounds);
            Players = new ObservableCollection<PlayerModel?>(_gameManager.GetPlayersFromRoom());
            Host = _gameManager.GetHostPlayerFromRoom();

            // ToDo: Test. Remove
            //_game.CurrentRoundLevel = RoundsLevelEnum.Final;

            OnChangeRound();
        }

        protected override async Task GoBackOrderAsync()
        {
            if (IsGameStarted)
            {
                ConfirmationResultEnum result = await _confirmationService.ShowInfoAsync(
                    "Подтверждение",
                    "Хотите вернутся в комнату? Игра будет завершена.",
                    ConfirmationResultEnum.Yes | ConfirmationResultEnum.No);

                if (result == ConfirmationResultEnum.Yes)
                {
                    _gameManager.CloseGame();
                    ClearAllParameters();
                }

                RegionManager.RequestNavigate(GameRegionNameService.GameMainLayerRegionName, nameof(RoomView));

                return;
            }

            MoveBackCommand.Execute(default);
        }

        /// <summary>
        /// Отобразить окно "Список тем"
        /// </summary>
        private void OnShowAllTopicsView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, Rounds
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.GameTopLayerRegionName, nameof(AllTopicsNameView), parameter);

            IsGameStarted = true;
        }

        /// <summary>
        /// Отобразить название текущего раунда
        /// </summary>
        private void OnShowRoundLevelNameView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, CurrentRound
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.GameTopLayerRegionName, nameof(RoundLevelNameView), parameter);
        }

        /// <summary>
        /// Отобразить окно "Список тем текущего раунда"
        /// </summary>
        private void OnShowTopicsCarouselView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _topics
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.GameTopLayerRegionName, nameof(TopicsNameCarouselControlView), parameter);

            IsShowedTopics = true;
            OnShowCurrentRoundView();
            SetPlayerFirstChoosingTopic();
        }

        /// <summary>
        /// Отобразить окно "Текущий раунд"
        /// </summary>
        private void OnShowCurrentRoundView()
        {
            if (_game == null)
            {
                return;
            }

            if (_game.CurrentRoundLevel == RoundsLevelEnum.Final)
            {
                // Подготовь финальный раунд
                Task.Run(async () => await PrepareFinalRound());

                RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(FinalRoundControlView));
            }
            else
            {
                RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(BaseRoundControlView));
            }
        }

        /// <summary>
        /// Показать вопрос для ответа
        /// </summary>
        private void OnShowQuestionForAnswerView()
        {
            if (_displayedQuestion == null)
            {
                return;
            }

            if (_currentRound?.Level == RoundsLevelEnum.Final)
            {
                RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(FinalRoundDisplayedQuestionView));

                return;
            }

            // ToDo: move to button click action
            // Игра готова принимать ответы
            // GameIsReadyToReceiveAnswers(true);

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(DisplayedQuestionView));
        }

        /// <summary>
        /// Показать правильный ответ
        /// </summary>
        private void OnShowCorrectAnswerView()
        {
            if (_displayedQuestion == null)
            {
                return;
            }

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(BaseCorrectAnswerView));
        }

        /// <summary>
        /// Показать ставки игроков
        /// </summary>
        private void OnShowPlayersBetView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _playerBetModels
                },
                {
                    NavigationParameterService.InitializeSecondParameter, _displayedQuestion
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(FinalRoundPlayersBetAndAnswerView), parameter);
        }

        /// <summary>
        /// Показать победителя игры
        /// </summary>
        private void OnShowGameWinnerView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _players
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(GameWinnerView), parameter);
        }
    }
}