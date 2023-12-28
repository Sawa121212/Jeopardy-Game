using System.Collections.Generic;
using DataDomain.Rooms;

namespace DataDomain
{
    /// <summary>
    /// Комната
    /// </summary>
    public class Room
    {
        public Room(string key)
        {
            Key = key;
            Players = new List<Player>();
        }

        public List<Player> Players { get; set; }
        public Player Host { get; set; }
        public Game? Game { get; set; }
        public string Key { get; }
    }
}