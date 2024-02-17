using ReactiveUI;
using Users.Domain.Models;

namespace Game.Ui.Models
{
    public class InvitationModelExtended : ReactiveObject
    {
        public InvitationModelExtended(User user)
        {
            User = user;
        }

        public User User
        {
            get => _user;
            init => this.RaiseAndSetIfChanged(ref _user, value);
        }

        public bool IsInvited
        {
            get => _isInvited;
            set => this.RaiseAndSetIfChanged(ref _isInvited, value);
        }

        private readonly User _user;
        private bool _isInvited;
    }
}