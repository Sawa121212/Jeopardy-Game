using Telegram.Bot.Types;

namespace GameSender.Infrastructure.Interfaces;

public interface IGameSenderService
{
    bool IsReady();

    Task<bool> SendAnInvitation(long playerId);

    // Task<bool> SendConnectedPlayerActions(long playerId);
    Task<bool> SendKickedMessage(long playerId);
    Task<bool> SendLoseGameInFinalRound(long playerId);
    Task<bool> SendBaseGameButton(long playerId, string text);
    Task<bool> SendRedButton(long playerId);

    Task<Message?> SendMessageAsync(long playerId, string text);
    Task<Message?> ForwardMessageAsync(long playerId, long pictureChatId, long pictureMessageId);
    Task<object> ParseMessageAsync(Message? message);
}