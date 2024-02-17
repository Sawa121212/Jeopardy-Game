﻿using DataDomain.Rooms;
using ReactiveUI;

namespace Game.Ui.Models
{
    public class PlayerBetModel : ReactiveObject
    {
        public PlayerBetModel(PlayerModel? playerModel)
        {
            PlayerModel = playerModel;
        }

        public PlayerModel? PlayerModel
        {
            get => _playerModel;
            private init => this.RaiseAndSetIfChanged(ref _playerModel, value);
        }

        /// <summary>
        /// Ставка
        /// </summary>
        public int Bet
        {
            get => _bet;
            set => this.RaiseAndSetIfChanged(ref _bet, value);
        }

        /// <summary>
        /// Игрок сделал ставку
        /// </summary>
        public bool IsMadeBet
        {
            get => _isInvited;
            set => this.RaiseAndSetIfChanged(ref _isInvited, value);
        }

        private readonly PlayerModel? _playerModel;
        private int _bet;
        private bool _isInvited;
    }
}