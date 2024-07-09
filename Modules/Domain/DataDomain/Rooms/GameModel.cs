using System.Collections.Generic;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using ReactiveUI;

namespace DataDomain.Rooms
{
    /// <summary>
    /// Модель игры
    /// </summary>
    public class GameModel : ReactiveObject
    {
        public GameModel()
        {
            CurrentRoundLevel = RoundsLevelEnum.Round1;
        }

        public RoundsLevelEnum CurrentRoundLevel
        {
            get => _currentRound;
            set => this.RaiseAndSetIfChanged(ref _currentRound, value);
        }

        public List<RoundModel?>? Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        public bool IsStarted
        {
            get => _isStarted;
            set => this.RaiseAndSetIfChanged(ref _isStarted, value);
        }

        private RoundsLevelEnum _currentRound;
        private List<RoundModel?>? _rounds;
        private bool _isStarted;
    }
}