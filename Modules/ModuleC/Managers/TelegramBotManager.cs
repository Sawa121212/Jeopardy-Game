using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Components;
using Common.Extensions;
using ReactiveUI;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramAPI.Test.Helpers;
using TelegramAPI.Test.Services.Settings;
using Users.Infrastructure;

namespace TelegramAPI.Test.Managers
{
    public class TelegramBotManager : ReactiveObject, ITelegramBotManager
    {
        public TelegramBotManager(ITelegramSettingsService telegramSettingsService)
        {
            _telegramSettingsService = telegramSettingsService;
            _token = _telegramSettingsService.GetGameBotToken();


            _receiverOptions = new ReceiverOptions // Также присваиваем значение настройкам бота
            {
                // Тут указываем типы получаемых Update`ов, о них подробнее рассказано тут https://core.telegram.org/bots/api#update
                AllowedUpdates = new[]
                {
                    UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабатывать
                ThrowPendingUpdates = true,
            };

            Task.Run(async () => await StartTelegramBot(_token));
        }

        public async Task<Result<bool>> StartTelegramBot(string token)
        {
            string errorMessage = null;
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return Result<bool>.Fail("Токен пуст");
                }

                _telegramBotClient = new TelegramBotClient(token);
                using CancellationTokenSource cts = new();

                // UpdateHander - обработчик приходящих Update`ов
                // ErrorHandler - обработчик ошибок, связанных с Bot API
                // Запускаем бота
                _telegramBotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

                // Создаем переменную, в которую помещаем информацию о нашем боте.
                User me = await _telegramBotClient.GetMeAsync(cancellationToken: cts.Token);
                //Messages.Add($"{me.FirstName} запущен!");
                Debug.Write($"{me.FirstName} запущен!");

                // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
                //await Task.Delay(-1, cts.Token);

                return Result<bool>.Done(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errorMessage = $"TelegramBot: {e.Message}";
            }

            return Result<bool>.Fail(errorMessage);
        }

        public virtual async Task UpdateHandler(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            // Эта переменная будет содержать в себе все связанное с сообщениями
            Message? message = update?.Message;
            // From - это от кого пришло сообщение (или любой другой Update)
            User? user = message?.From;
            // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
            try
            {

                if (_userService.GetUserById(user.Id) == null)
                {
                    var user_ = _userService.CreateUser(user.Id, $"{user.FirstName} {user.LastName}");
                }
                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                switch (update.Type)
                {
                    case UpdateType.Message:
                    {
                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        //Messages.Add($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        // Chat - содержит всю информацию о чате
                        Chat chat = message.Chat;

                        // Добавляем проверку на тип Message
                        
                            switch (message.Type)
                        {
                            // Тут понятно, текстовый тип
                            case MessageType.Text:
                            {
                                // тут обрабатываем команду /start, остальные аналогично
                                if (message.Text == "/start")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Выбери клавиатуру:\n" +
                                        "/inline\n" +
                                        "/reply\n", cancellationToken: cancellationToken);
                                    return;
                                }

                                if (message.Text == "/inline")
                                {
                                    // Тут создаем нашу клавиатуру
                                    InlineKeyboardMarkup inlineKeyboard = new(
                                        new
                                            List<InlineKeyboardButton
                                                []>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                            {
                                                // Каждый новый массив - это дополнительные строки,
                                                // а каждая дополнительная кнопка в массиве - это добавление ряда

                                                new InlineKeyboardButton[] // тут создаем массив кнопок
                                                {
                                                    InlineKeyboardButton.WithUrl("Это кнопка с сайтом",
                                                        "https://habr.com/"),
                                                    InlineKeyboardButton.WithCallbackData("А это просто кнопка",
                                                        "button1"),
                                                },
                                                new InlineKeyboardButton[]
                                                {
                                                    InlineKeyboardButton.WithCallbackData("Тут еще одна", "button2"),
                                                    InlineKeyboardButton.WithCallbackData("И здесь", "button3"),
                                                },
                                            });

                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Это inline клавиатура!",
                                        replyMarkup:
                                        inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup

                                    return;
                                }

                                if (message.Text == "/reply")
                                {
                                    // Тут все аналогично Inline клавиатуре, только меняются классы
                                    // НО! Тут потребуется дополнительно указать один параметр, чтобы
                                    // клавиатура выглядела нормально, а не как абы что

                                    ReplyKeyboardMarkup replyKeyboard = new(
                                        new List<KeyboardButton[]>()
                                        {
                                            new KeyboardButton[]
                                            {
                                                new("Привет!"), new("Пока!"),
                                            },
                                            new KeyboardButton[]
                                            {
                                                new("Позвони мне!")
                                            },
                                            new KeyboardButton[]
                                            {
                                                new("Напиши моему соседу!")
                                            }
                                        })
                                    {
                                        // автоматическое изменение размера клавиатуры, если не стоит true,
                                        // тогда клавиатура растягивается чуть ли не до луны,
                                        // проверить можете сами
                                        ResizeKeyboard = true,
                                    };

                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Это reply клавиатура!",
                                        replyMarkup: replyKeyboard,
                                        cancellationToken:
                                        cancellationToken); // опять передаем клавиатуру в параметр replyMarkup

                                    return;
                                }

                                if (message.Text == "Позвони мне!")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Хорошо, присылай номер!",
                                        replyToMessageId: message.MessageId, cancellationToken: cancellationToken);

                                    return;
                                }

                                if (message.Text == "Напиши моему соседу!")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "А самому что, трудно что-ли ?",
                                        replyToMessageId: message.MessageId, cancellationToken: cancellationToken);

                                    return;
                                }

                                if (!message.Text.IsNullOrEmpty())
                                {
                                    await CheckAddedAdminMode(chat.Id, message.Text);
                                    return;
                                }

                                return;
                            }

                            // Добавил default , чтобы показать вам разницу типов Message
                            default:
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Используй только текст!", cancellationToken: cancellationToken);
                                return;
                            }
                        }

                        return;
                    }

                    case UpdateType.CallbackQuery:
                    {
                        // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                        CallbackQuery? callbackQuery = update.CallbackQuery;

                        // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                        user = callbackQuery.From;

                        // Выводим на экран нажатие кнопки
                        //Messages.Add($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                        // Вот тут нужно уже быть немножко внимательным и не путаться!
                        // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                        // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                        Chat chat = callbackQuery.Message.Chat;

                        // Добавляем блок switch для проверки кнопок
                        switch (callbackQuery.Data)
                        {
                            // Data - это придуманный нами id кнопки, мы его указывали в параметре
                            // callbackData при создании кнопок. У меня это button1, button2 и button3

                            case "button1":
                            {
                                // В этом типе клавиатуры обязательно нужно использовать следующий метод
                                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    $"Вы нажали на {callbackQuery.Data}");
                                return;
                            }

                            case "button2":
                            {
                                // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Тут может быть ваш текст!");

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    $"Вы нажали на {callbackQuery.Data}");
                                return;
                            }

                            case "button3":
                            {
                                // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!",
                                    showAlert: true);

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    $"Вы нажали на {callbackQuery.Data}");
                                return;
                            }
                        }

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //Messages.Add(ex.ToString());
            }
        }

        private async Task SendMessage(long chatId, string message)
        {
            // ToDo show Exception result
            try
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task CheckAddedAdminMode(long chatId, string key)
        {
            if (!IsAddAdminMode)
            {
                return;
            }

            if (key.IsNullOrEmpty())
            {
                return;
            }

            if (key == _adminModeKey)
            {
                await _telegramSettingsService.SetAdminUserIdToken(chatId.ToString());
                CancelAddAdminMode();
                await SendMessage(chatId, $"Вы стали администратором");
            }
        }

        public bool IsConnected()
        {
            return false;
        }

        public bool IsAddAdminMode
        {
            get => _isAddAdminMode;
            private set => this.RaiseAndSetIfChanged(ref _isAddAdminMode, value);
        }

        /// <inheritdoc />
        public TelegramBotClient TelegramBotClient => (TelegramBotClient)_telegramBotClient;

        /// <inheritdoc />
        public async Task<string> VerifyAddAdminMode(long chatId)
        {
            await _telegramSettingsService.SetAdminUserIdToken(null).ConfigureAwait(true);

            _adminModeKey = RandomNumberGenerator.GenerateFormattedSixDigitRandomNumber();
            IsAddAdminMode = true;
            await SendMessage(chatId, $"Вторая часть кода подтверждения: ***{_adminModeKey.Substring(3, 3)}");
            return _adminModeKey.Substring(0, 3);
        }

        /// <inheritdoc />
        public void CancelAddAdminMode()
        {
            _adminModeKey = null;
            IsAddAdminMode = false;
        }

        private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            string errorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            //Debug.Fail(errorMessage);
            //Messages.Add(errorMessage);
            return Task.CompletedTask;
        }

        private readonly ITelegramSettingsService _telegramSettingsService;
        private readonly string? _token;

        // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
        private ITelegramBotClient _telegramBotClient;
        private readonly IUserService _userService;

        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        private readonly ReceiverOptions _receiverOptions;
        private bool _isAddAdminMode;
        private string _adminModeKey;
    }
}