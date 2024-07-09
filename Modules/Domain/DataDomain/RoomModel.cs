using System.Collections.Generic;
using DataDomain.Rooms;
using ReactiveUI;

namespace DataDomain
{
    /// <summary>
    /// Комната
    /// </summary>
    public class RoomModel : ReactiveObject
    {
        public RoomModel(string key)
        {
            Key = key;
            Players = new List<PlayerModel?>();
        }

        public List<PlayerModel?> Players
        {
            get => _players;
            private set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        public PlayerModel? Host
        {
            get => _host;
            set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public GameModel? Game
        {
            get => _game;
            set => this.RaiseAndSetIfChanged(ref _game, value);
        }

        public string Key
        {
            get => _key;
            private set => this.RaiseAndSetIfChanged(ref _key, value);
        }

        private List<PlayerModel?> _players;
        private PlayerModel? _host;
        private GameModel? _game;
        private string _key;
    }
}