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
using TelegramAPI.Test.Managers;
using File = System.IO.File;

namespace TelegramAPI.Test.Views
{
    public class TelegramTestViewModel : BindableBase
    {
        private readonly ITelegramBotManager _telegramBotManager;

        private ObservableCollection<string> _messages;

        public TelegramTestViewModel(ITelegramBotManager telegramBotManager)
        {
            _telegramBotManager = telegramBotManager;
            Messages = new ObservableCollection<string>();
            _botClient = _telegramBotManager.TelegramBotClient;
        }

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
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
                                        cancellationToken: cancellationToken); // опять передаем клавиатуру в параметр replyMarkup

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

        private readonly ITelegramBotClient _botClient;
    }
}