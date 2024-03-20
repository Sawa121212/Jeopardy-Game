using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using Game.Ui.Models;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;

namespace Game.Ui.Views.GameControls.Pages
{
    /// <summary>
    /// Отправка приглашений на вход в комнату
    /// </summary>
    public class SendAnInvitationControlViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public SendAnInvitationControlViewModel(
            IRegionManager regionManager,
            IUserService userService,
            ITelegramBotService telegramBotService)
            : base(regionManager)
        {
            _userService = userService;
            _telegramBotService = telegramBotService;
            SendAnInvitationCommand = new DelegateCommand<InvitationModelExtended?>(async (u) => await OnSendAnInvitation(u));
            SendAnInvitationEveryoneCommand = new DelegateCommand(async () => await OnSendAnInvitationEveryone());
        }

        ObservableCollection<InvitationModelExtended> Users
        {
            get => _users;
            set => this.RaiseAndSetIfChanged(ref _users, value);
        }

        public ICommand SendAnInvitationCommand { get; }
        public ICommand SendAnInvitationEveryoneCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // Initialize parameter
            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];

            IList<PlayerModel>? playersInRoom = parameter as IList<PlayerModel>;


            Users = new ObservableCollection<InvitationModelExtended>();
            foreach (User? user in _userService.GetAllUsers())
            {
                if (playersInRoom?.FirstOrDefault(p => p.Id == user.Id) != null)
                {
                    continue;
                }

                Users.Add(new InvitationModelExtended(user));
            }

            // Test.Remove
            Users.Add(
                new InvitationModelExtended(new User()
                    {
                        Id = 12831023,
                        Name = "sdsdsd"
                    }
                ));
            Users.Add(
                new InvitationModelExtended(new User()
                    {
                        Id = 938969260,
                        Name = "Sawa"
                    }
                ));
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Users = null;
        }

        /// <summary>
        /// Отправить приглашение всем
        /// </summary>
        /// <returns></returns>
        private async Task OnSendAnInvitationEveryone()
        {
            foreach (InvitationModelExtended invitationModelExtended in _users)
            {
                await OnSendAnInvitation(invitationModelExtended);
            }
        }

        /// <summary>
        /// Отправить приглашение
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private async Task OnSendAnInvitation(InvitationModelExtended? userModel)
        {
            if (userModel == null || userModel.User == null || userModel.IsInvited)
            {
                return;
            }

            await _telegramBotService.SendMessageAsync(userModel.User.Id, "Вас пригласили в комнату.");

            // ToDo: send button

            userModel.IsInvited = true;
        }

        private readonly IUserService _userService;
        private readonly ITelegramBotService _telegramBotService;
        private ObservableCollection<InvitationModelExtended> _users;
    }
}