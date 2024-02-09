using Avalonia.Media.Imaging;

namespace TelegramAPI.Test.Models
{
    public class MessageModel
    {
        public MessageModel(string text, Bitmap bitmap = null)
        {
            Text = text;
            Bitmap = bitmap;
        }

        public string? Text { get; }
        public Bitmap? Bitmap { get; }

        //public Audio? Audio { get; }
    }
}