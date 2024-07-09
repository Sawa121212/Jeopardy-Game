using ReactiveUI;
using Users.Domain.Models;

namespace DataDomain.Rooms
{
    public class PlayerModel : ReactiveObject
    {
        public PlayerModel()
        {
            Name = "Test";
        }

        public PlayerModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public long Id
        {
            get => _id;
            init => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            init => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public int Points
        {
            get => _points;
            set => this.RaiseAndSetIfChanged(ref _points, value);
        }

        public void AddPoint(int value) => Points += value;

        private long _id;
        private string _name;
        private int _points;
    }
}