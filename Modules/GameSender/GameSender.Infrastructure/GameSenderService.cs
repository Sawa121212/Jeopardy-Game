using GameSender.Infrastructure.Interfaces;
using Telegram.Bot.Types;
using TelegramAPI.Infrastructure.Interfaces.Managers;

namespace GameSender.Infrastructure;

public partial class GameSenderService : IGameSenderService
{
    private readonly ITelegramBotManager _telegramBotManager;
    private readonly ITelegramBotService _telegramBotService;

    public GameSenderService(ITelegramBotManager telegramBotManager, ITelegramBotService telegramBotService)
    {
        _telegramBotManager = telegramBotManager;
        _telegramBotService = telegramBotService;
    }

    /// <inheritdoc />
    public bool IsReady()
    {
        return _telegramBotManager.IsConnected;
    }

    /// <inheritdoc />
    public async Task<bool> SendAnInvitation(long userId)
    {
        if (!IsReady())
        {
            return false;
        }

        await _telegramBotService.SendMessageAsync(userId, "Вас пригласили в комнату", SendAnInvitationButton);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> SendConnectedPlayerActions(long userId)
    {
        if (!IsReady())
        {
            return false;
        }

        Message? result = await _telegramBotService.SendMessageAsync(userId, "Вы можете:", BaseRoomButtons);

        return result is not null;
    }

    /// <inheritdoc />
    public async Task<bool> SendKickedMessage(long playerId)
    {
        if (!IsReady())
        {
            return false;
        }

        await _telegramBotService.SendMessageAsync(playerId, $"Вас кикнули с комнаты", EmptyButtons);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> SendLoseGameInFinalRound(long playerId)
    {
        if (!IsReady())
        {
            return false;
        }

        await _telegramBotService.SendMessageAsync(playerId,
            $"Финальный раунд: вы завершаете игру и терпите досрочное поражение. \nОжидайте конца игры",
            EmptyButtons);
        return true;
    }

    /// <inheritdoc />
    public async Task<Message?> SendMessageAsync(long playerId, string text)
    {
        if (!IsReady())
        {
            return null;
        }

        return await _telegramBotService.SendMessageAsync(playerId, text, EmptyButtons);
    }

    /// <inheritdoc />
    public async Task<Message?> ForwardMessageAsync(long playerId, long pictureChatId, long pictureMessageId)
    {
        if (!IsReady())
        {
            return null;
        }

        Message? result = await _telegramBotService.ForwardMessageAsync(playerId, pictureChatId, pictureMessageId);
        await _telegramBotService.SendMessageAsync(playerId, null, EmptyButtons);
        return result;
    }

    /// <inheritdoc />
    public async Task<object> ParseMessageAsync(Message? message)
    {
        if (!IsReady())
        {
            return null;
        }

        return await _telegramBotService.ParseMessageAsync(message);
    }
}