using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Components;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Confirmation.Module.Enums;
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
        public ICommand ShowGameTopicsCommand { get; }

        public ICommand ShowTopicsCarouselCommand { get; }

        public ICommand MoveBackButtonCommand { get; }

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
                    _gameManager.CloseGame(_roomKey);
                    ClearAllParameters();
                }

                RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(RoomView));

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

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(AllTopicsNameView), parameter);

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

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(RoundLevelView), parameter);
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

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(TopicsNameCarouselControlView), parameter);
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
            GameIsReadyToReceiveAnswers(true);

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