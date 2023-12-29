using System.Collections.Generic;

namespace DataDomain.Rooms.Rounds
{
    public class Topic
    {
        public Topic()
        {
            Questions = new List<Question>(5);
        }

        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Вопросы
        /// </summary>
        public List<Question> Questions { get; set; }
    }
}