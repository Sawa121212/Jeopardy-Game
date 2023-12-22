using Common.Core.Interfaces.Settings;
using ProtoBuf;

namespace TelegramAPI.Test.Settings
{
    [ProtoContract]
    public class TelegramSettings : ISettings
    {
        [ProtoMember(1)]
        private string _gameBotToken;

        [ProtoMember(2)]
        private string _adminUserId;


        public string GameBotToken
        {
            get => _gameBotToken;
            set => _gameBotToken = value;
        }

        public string AdminUserId
        {
            get => _adminUserId;
            set => _adminUserId = value;
        }
    }
}