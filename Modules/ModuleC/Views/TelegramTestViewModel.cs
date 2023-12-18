using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Prism.Mvvm;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramAPI.Test.Views
{
    public class TelegramTestViewModel : BindableBase
    {
        // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
        private readonly ITelegramBotClient _botClient;

        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        private readonly ReceiverOptions _receiverOptions;
        private readonly string? _token;
        private ObservableCollection<string> _messages;
        private string _tokenFile = @"Settings\TelegramToken.tmp";

        public TelegramTestViewModel()
        {
            Messages = new ObservableCollection<string>();

            if (File.Exists(_tokenFile))
            {
                _token = File.ReadAllText(_tokenFile);
            }
            else
            {
                Messages.Add("File Not Exists");
                return;
            }

            // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
            if (string.IsNullOrEmpty(_token))
            {
                Messages.Add("Token Is Empty");
                return;
            }

            _botClient = new TelegramBotClient(_token);
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

            Initialize();
        }

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        private async Task Initialize()
        {
            using CancellationTokenSource cts = new();

            // UpdateHander - обработчик приходящих Update`ов
            // ErrorHandler - обработчик ошибок, связанных с Bot API
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

            User me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
            Messages.Add($"{me.FirstName} запущен!");

            await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
            try
            {
                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                switch (update.Type)
                {
                    case UpdateType.Message:
                    {
                        // Эта переменная будет содержать в себе все связанное с сообщениями
                        Message? message = update.Message;

                        // From - это от кого пришло сообщение (или любой другой Update)
                        User? user = message.From;

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        Messages.Add($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

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
                                        "/reply\n");
                                    return;
                                }

                                if (message.Text == "/inline")
                                {
                                    // Тут создаем нашу клавиатуру
                                    InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                                        new
                                            List<InlineKeyboardButton
                                                []>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                            {
                                                // Каждый новый массив - это дополнительные строки,
                                                // а каждая дополнительная кнопка в массиве - это добавление ряда

                                                new InlineKeyboardButton[] // тут создаем массив кнопок
                                                {
                                                    InlineKeyboardButton.WithUrl("Это кнопка с сайтом", "https://habr.com/"),
                                                    InlineKeyboardButton.WithCallbackData("А это просто кнопка", "button1"),
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
                                        replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup

                                    return;
                                }

                                if (message.Text == "/reply")
                                {
                                    // Тут все аналогично Inline клавиатуре, только меняются классы
                                    // НО! Тут потребуется дополнительно указать один параметр, чтобы
                                    // клавиатура выглядела нормально, а не как абы что

                                    ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(
                                        new List<KeyboardButton[]>()
                                        {
                                            new KeyboardButton[]
                                            {
                                                new KeyboardButton("Привет!"), new KeyboardButton("Пока!"),
                                            },
                                            new KeyboardButton[]
                                            {
                                                new KeyboardButton("Позвони мне!")
                                            },
                                            new KeyboardButton[]
                                            {
                                                new KeyboardButton("Напиши моему соседу!")
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
                                        replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

                                    return;
                                }

                                if (message.Text == "Позвони мне!")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Хорошо, присылай номер!",
                                        replyToMessageId: message.MessageId);

                                    return;
                                }

                                if (message.Text == "Напиши моему соседу!")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "А самому что, трудно что-ли ?",
                                        replyToMessageId: message.MessageId);

                                    return;
                                }

                                return;
                            }

                            // Добавил default , чтобы показать вам разницу типов Message
                            default:
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Используй только текст!");
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
                        User user = callbackQuery.From;

                        // Выводим на экран нажатие кнопки
                        Messages.Add($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

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
                                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!", showAlert: true);

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
                Messages.Add(ex.ToString());
            }
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

            Messages.Add(errorMessage);
            return Task.CompletedTask;
        }
    }
}