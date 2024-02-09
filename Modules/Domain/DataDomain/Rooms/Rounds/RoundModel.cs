using System.Collections.Generic;
using Avalonia.Media;
using Common.Extensions;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using ReactiveUI;

namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Раунд
    /// </summary>
    public class RoundModel : ReactiveObject
    {
        public RoundModel(RoundsLevelEnum level)
        {
            Level = level;
            Background = RoundHelper.GetRoundColor(level);
        }

        /// <summary>
        /// Номер раунда
        /// </summary>
        public RoundsLevelEnum Level
        {
            get => _level;
            init => this.RaiseAndSetIfChanged(ref _level, value);
        }

        /// <summary>
        /// Имя раунда. 1, 2, 3 или Финальный раунд
        /// </summary>
        public string Name => Level.GetDescription();

        /// <summary>
        /// Темы вопросов
        /// </summary>
        public List<TopicModel>? Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public SolidColorBrush Background
        {
            get => _background;
            init => this.RaiseAndSetIfChanged(ref _background, value);
        }

        private readonly RoundsLevelEnum _level;
        private List<TopicModel>? _topics;
        private readonly SolidColorBrush _background;
    }
}