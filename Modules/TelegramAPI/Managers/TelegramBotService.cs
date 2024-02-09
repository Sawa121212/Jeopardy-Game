using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramAPI.Test.Models;
using File = Telegram.Bot.Types.File;

namespace TelegramAPI.Test.Managers
{
    public class TelegramBotService : ReactiveObject, ITelegramBotService
    {
        public TelegramBotService(ITelegramBotManager telegramBotManager)
        {
            _telegramBotManager = telegramBotManager;
        }

        /// <inheritdoc />
        public async Task<Message?> SendMessageAsync(long chatId, string text)
        {
            // ToDo show Exception result
            try
            {
                Message? message = await _telegramBotManager.TelegramBotClient.SendTextMessageAsync(chatId, text);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Message?> SendPhotoAsync(long chatId, string photoUrl, string caption = "", int replyToMessageId = 0)
        {
            try
            {
                Message message;
                await using FileStream fileStream = new(photoUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                message = await _telegramBotManager.TelegramBotClient.SendPhotoAsync(chatId,
                    new InputFileStream(fileStream), null, caption);

                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Message?> ForwardMessageAsync(long chatId, long fromChatId, long messageId)
        {
            // ToDo show Exception result
            try
            {
                Message? message = await _telegramBotManager.TelegramBotClient.ForwardMessageAsync(chatId, fromChatId, (int) messageId);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<MessageModel?> ParseMessageAsync(Message message)
        {
            if (message == null)
            {
                return null;
            }

            try
            {
                string? text = null;
                Bitmap? bitmap = null;
                if (message.Photo != null)
                {
                    bitmap = await CreateBitmapAsync(message);
                    text = message.Caption;
                }

                return new MessageModel(text ?? message.Text, bitmap);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
        private async Task<Bitmap?> CreateBitmapAsync(Message message)
        {
            try
            {
                if (message.Photo != null)
                {
                    string largestPhoto = message.Photo![^1].FileId;
                    File photoFile = await _telegramBotManager.TelegramBotClient.GetFileAsync(largestPhoto);

                    if (photoFile.FilePath != null)
                    {
                        string downloadFolder = "Download";
                        string photosDownloadFolder = "Download/photos";
                        if (!Directory.Exists(photosDownloadFolder))
                        {
                            Directory.CreateDirectory(photosDownloadFolder);
                        }

                        string filename = $"{downloadFolder}/{photoFile.FilePath}";

                        await using (FileStream saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                        {
                            await _telegramBotManager.TelegramBotClient.DownloadFileAsync(photoFile.FilePath, saveImageStream);
                        }

                        await using FileStream fileStream = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                        return new Bitmap(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }

        private readonly ITelegramBotManager _telegramBotManager;
    }
}