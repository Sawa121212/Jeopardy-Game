using System.Collections.Generic;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;

namespace Game.Services
{
    public class RoundService : IRoundService
    {
        /// <inheritdoc />
        public List<Round> CreateGameRounds()
        {
            List<Round> rounds = new()
            {
                CreateRound(RoundsLevelEnum.Round1),
                CreateRound(RoundsLevelEnum.Round2),
                CreateRound(RoundsLevelEnum.Round3),
                CreateRound(RoundsLevelEnum.Final)
            };

            return rounds;
        }

        private Round CreateRound(RoundsLevelEnum levelEnum)
        {

            // 1. topic
            // 2. questions
            // 3. round

            Round round = new(levelEnum)
            {
                Topics = { }
            };



            new Topic()
            {
                Questions = new List<Question>()
                {
                    new()
                }
            };
            return round;
        }

        private Topic GetRandomTopic()
        {
            return new Topic();
        }
    }
}