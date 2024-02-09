using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Game.Domain.Data;
using Game.Ui.Views.GameControls.GamePages;
using Game.Ui.Views.GameControls.GamePages.Rounds;
using Game.Ui.Views.GameControls.GamePages.Topics;
using Prism.Regions;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand ShowGameTopicsCommand { get; }

        public ICommand ShowTopicsCarouselCommand { get; }

        /// <summary>
        /// Отобразить окно "Список тем"
        /// </summary>
        private void OnShowAllTopics()
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
        private void OnShowRoundLevel()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, CurrentRound
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(RoundLevelView), parameter);
        }

        /// <summary>
        /// Отобразить окно "Список тем текущего раунда"
        /// </summary>
        private void OnShowTopicsCarousel()
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
        private void OnShowCurrentRound()
        {
            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(BaseRoundControlView));
        }

        /// <summary>
        /// Показать вопрос для ответа
        /// </summary>
        private void OnShowQuestionForAnswer()
        {
            if (_displayedQuestion == null)
            {
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
        private void OnShowCorrectAnswer()
        {
            if (_displayedQuestion == null)
            {
                return;
            }

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(CorrectAnswerView));
        }

        /// <summary>
        /// Показать победителя игры
        /// </summary>
        private void OnShowGameWinner()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _players
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.ContentRegionName, nameof(GameWinnerView), parameter);

            ClearAllParameters();
        }
    }
}