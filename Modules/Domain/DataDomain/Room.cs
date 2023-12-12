using System.Collections.Generic;
using DataDomain.Rooms;

namespace DataDomain
{
    /// <summary>
    /// Комната
    /// </summary>
    public class Room
    {
        public Room()
        {
            Key = "123"; // ToDo RandomKey
        }

        public string Key { get; }
        public List<Player> Players { get; set; }
        public Player Host { get; set; }
        public Game Game { get; set; }

        // ToDo: extract methods to Services
        public void SetHost(Player player)
        {
            if (!Players.Contains(player))
            {
                return;
            }

            if (Host != null)
            {
                Players.Add(Host);
            }

            Players.Remove(player);
            Host = player;
        }

        public void AddPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                return;
            }

            Players.Add(player);
        }
    }
}