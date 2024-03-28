using System;
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
using TelegramAPI.Infrastructure.Interfaces.Managers;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using User = Telegram.Bot.Types.User;

namespace TelegramAPI.Infrastructure.Managers
{
    public class TelegramBotManager : ReactiveObject, ITelegramBotManager
    {
        public TelegramBotManager(
            ITelegramSettingsService telegramSettingsService,
            ITelegramHandlerService telegramHandlerService,
            IUserService userService)
        {
            _userService = userService;
            _telegramSettingsService = telegramSettingsService;
            _telegramHandlerService = telegramHandlerService;

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

            //Task.Run(async () => await StartTelegramBot(_token).ConfigureAwait(true));
        }

        /// <inheritdoc />
        public async Task<Result<bool>> StartTelegramBot()
        {
            string errorMessage = null;
            _token = _telegramSettingsService.GetGameBotToken();

            try
            {
                await StartTelegramBot(_token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errorMessage = $"TelegramBot: {e.Message}";
            }

            return Result<bool>.Fail(errorMessage);
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

                // UpdateHandler - обработчик приходящих Update`ов
                // ErrorHandler - обработчик ошибок, связанных с Bot API
                // Запускаем бота
                _telegramBotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

                // Создаем переменную, в которую помещаем информацию о нашем боте.
                User me = await _telegramBotClient.GetMeAsync(cancellationToken: cts.Token);

                //Messages.Add($"{me.FirstName} запущен!");
                Debug.Write($"[Debug] TelegramBot: {me.FirstName} запущен!");

                // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
                //await Task.Delay(-1, cts.Token);
                //this.RaisePropertyChanged(nameof(IsConnected));
                IsConnected = true;
                return Result<bool>.Done(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errorMessage = $"TelegramBot: {e.Message}";
            }

            return Result<bool>.Fail(errorMessage);
        }

        public virtual async Task UpdateHandler(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
            try
            {
                // Эта переменная будет содержать в себе все связанное с сообщениями
                Message? message = update?.Message;

                // From - это от кого пришло сообщение (или любой другой Update)
                User? user = message?.From;

                if (user == null)
                {
                    return;
                }

                if (!_userService.TryGetUserById(user.Id, out Users.Domain.Models.User _user))
                {
                    _user = _userService.CreateUser(user.Id, $"{user.FirstName} {user.LastName}", user.Username);
                    await botClient.SendTextMessageAsync(_user.Id, "Введите ваше имя");
                    return;
                }

                Result<Tuple<StateUserEnum, string>> result = _telegramHandlerService.Handle(update);

                if (result)
                {
                    _user.State = result.Value.Item1;

                    _userService.UpdateUser(_user);
                    var resultKeyboard = _telegramHandlerService.GetKeyboardMarkup(result.Value.Item1, update);
                    ReplyMarkupBase replyKeyboard = resultKeyboard ? resultKeyboard.Value : new ReplyKeyboardRemove();
                    await botClient.SendTextMessageAsync(_user.Id, result.Value.Item2, replyMarkup: replyKeyboard);
                }
                else
                {
                    await botClient.SendTextMessageAsync(_user.Id, result.ErrorMessage);
                    return;
                }

                return;

                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                /*switch (update.Type)
                {
                    case UpdateType.Message:
                        {
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
                }*/
            }
            catch (Exception ex)
            {
                //Messages.Add(ex.ToString());
            }
        }

        /// <inheritdoc />
        public bool IsConnected
        {
            get => _isConnected;
            set => this.RaiseAndSetIfChanged(ref _isConnected, value);
        }

        /// <inheritdoc />
        public TelegramBotClient TelegramBotClient => (TelegramBotClient) _telegramBotClient;

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
        private readonly ITelegramHandlerService _telegramHandlerService;
        private string? _token;

        // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
        private ITelegramBotClient _telegramBotClient;
        private readonly IUserService _userService;

        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        private readonly ReceiverOptions _receiverOptions;
        private bool _isConnected;
    }
}