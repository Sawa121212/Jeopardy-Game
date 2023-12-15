using System.Windows.Input;
using Common.Core.Interfaces;
using Common.Core.Signals;
using Confirmation.Module.Enums;
using Confirmation.Module.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Confirmation.Module.Views
{
    public class ConfirmationViewModel : BindableBase, IInitializable<DialogInfo>, IResult<ConfirmationResultEnum>
    {
        private DialogInfo _info;
        private ConfirmationResultEnum _result;
        private ISignal<bool?> _closeSignal;

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
            set => SetProperty(ref _result, value);
        }

        public DialogInfo Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public ISignal<bool?> CloseSignal
        {
            get => _closeSignal;
            set => SetProperty(ref _closeSignal, value);
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