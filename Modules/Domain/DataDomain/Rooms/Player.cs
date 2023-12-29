namespace DataDomain.Rooms
{
    public class Player
    {
        public Player() // ToDo: ctor with TelegramAccount)
        {
            Name = "Test";
        }

        //public TelegramAccount TelegramAccount { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public void AddPoint(int value) => Points += value;
    }
}