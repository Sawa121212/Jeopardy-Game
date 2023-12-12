using System.Windows.Input;
using Avalonia.Controls;
using Common.Core.Interfaces;
using Common.Core.Signals;
using Common.Core.Views;
using Confirmation.Domain.Enums;
using Confirmation.Domain.Models;
using Prism.Commands;
using ReactiveUI;

namespace Confirmation.Ui.Views
{
    public class ConfirmationViewModel : ViewModelBase, IInitializable<DialogInfo>, IResult<ConfirmationResultEnum>
    {
        private DialogInfo _info;
        private ConfirmationResultEnum _result;
        private ISignal<bool?> _closeSignal;
        protected Window _window;

        public ConfirmationViewModel()
        {
            OkCommand = new DelegateCommand(Ok);
            YesCommand = new DelegateCommand(Yes);
            NoCommand = new DelegateCommand(No);
            CancelCommand = new DelegateCommand(Cancel);
            EscCommand = new DelegateCommand(Esc);
            EnterCommand = new DelegateCommand(Enter);
            CloseSignal = new Signal<bool?>();
        }

        public void Initialize(DialogInfo param)
        {
            Info = param;
        }

        public ConfirmationResultEnum Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        public DialogInfo Info
        {
            get => _info;
            set => this.RaiseAndSetIfChanged(ref _info, value);
        }

        public ISignal<bool?> CloseSignal
        {
            get => _closeSignal;
            set => this.RaiseAndSetIfChanged(ref _closeSignal, value);
        }

        /// <inheritdoc />
        public ConfirmationResultEnum GetResult()
        {
            return _result;
        }

        private void Ok()
        {
            _result = ConfirmationResultEnum.Ok;
            CloseSignal.Raise(true);
        }

        private void Yes()
        {
            _result = ConfirmationResultEnum.Yes;
            CloseSignal.Raise(true);
        }

        private void No()
        {
            _result = ConfirmationResultEnum.No;
            CloseSignal.Raise(true);
        }

        private void Cancel()
        {
            _result = ConfirmationResultEnum.Cancel;
        }

        private void Esc()
        {
            if (Info.Buttons.HasFlag(ConfirmationResultEnum.Cancel))
            {
                _result = ConfirmationResultEnum.Cancel;
            }
            else if (Info.Buttons.HasFlag(ConfirmationResultEnum.No))
            {
                _result = ConfirmationResultEnum.No;
            }

            CloseSignal.Raise(true);
        }

        private void Enter()
        {
            if (Info.Buttons.HasFlag(ConfirmationResultEnum.Ok))
            {
                _result = ConfirmationResultEnum.Ok;
            }
            else if (Info.Buttons.HasFlag(ConfirmationResultEnum.Yes))
            {
                _result = ConfirmationResultEnum.Yes;
            }

            CloseSignal.Raise(true);
        }

        public ICommand OkCommand { get; }
        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EscCommand { get; }
        public ICommand EnterCommand { get; }
    }
}