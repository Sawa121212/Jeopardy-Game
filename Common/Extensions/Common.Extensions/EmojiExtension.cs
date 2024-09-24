namespace Common.Extensions;

public class EmojiExtension
{
    public static string EmojiMessageFormat(MessageEmoji emoji, string message)
    {
        return GetEmojiValue(emoji) + " " + message;
    }

    private static string GetEmojiValue(MessageEmoji emoji)
    {
        switch (emoji)
        {
            case MessageEmoji.Success:
                return SuccessEmoji;
            case MessageEmoji.Warning:
                return WarningEmoji;
            case MessageEmoji.Error:
                return ErrorEmoji;
            default:
                throw new ArgumentOutOfRangeException(nameof(emoji), emoji, null);
        }

        return null;
    }

    private static readonly string SuccessEmoji = "\u2705";
    private static readonly string WarningEmoji = "\u26a0";
    private static readonly string ErrorEmoji = "\u274c";
}