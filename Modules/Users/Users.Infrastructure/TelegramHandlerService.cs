using Common.Core.Components;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using User = Users.Domain.Models.User;

namespace Users.Infrastructure
{
    public class TelegramHandlerService : ITelegramHandlerService
    {
        Dictionary<StateUserEnum, Func<Update, Result<Tuple<StateUserEnum, string>>>> _dictionary;
        Dictionary<StateUserEnum, Func<Update, Result<ReplyKeyboardMarkup>>> _dictionaryReplyKeyboardMarkup;
        private readonly IUserService _userService;

        public TelegramHandlerService(IUserService userService)
        {
            _dictionary = new Dictionary<StateUserEnum, Func<Update, Result<Tuple<StateUserEnum, string>>>>();
            _dictionaryReplyKeyboardMarkup = new Dictionary<StateUserEnum, Func<Update, Result<ReplyKeyboardMarkup>>>();
            _userService = userService;
        }

        public ITelegramHandlerService RegisterHandler(
            StateUserEnum stateUser,
            Func<Update, Result<Tuple<StateUserEnum, string>>> handler,
            Func<Update, Result<ReplyKeyboardMarkup>> replyKeyboardMarkupGenerator)
        {
            _dictionary.Add(stateUser, handler);
            _dictionaryReplyKeyboardMarkup.Add(stateUser, replyKeyboardMarkupGenerator);
            return this;
        }

        public Result<Telegram.Bot.Types.User> GetUser(Update update)
        {
            Telegram.Bot.Types.User botUser = null;

            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    botUser = update.Message.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.InlineQuery:
                    botUser = update.InlineQuery.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChosenInlineResult:
                    botUser = update.ChosenInlineResult.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    botUser = update.CallbackQuery.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.EditedMessage:
                    botUser = update.EditedMessage.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChannelPost:
                    botUser = update.ChannelPost.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.EditedChannelPost:
                    botUser = update.EditedChannelPost.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ShippingQuery:
                    botUser = update.ShippingQuery.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PreCheckoutQuery:
                    botUser = update.PreCheckoutQuery.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PollAnswer:
                    botUser = update.PollAnswer.User;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.MyChatMember:
                    botUser = update.MyChatMember.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatMember:
                    botUser = update.ChatMember.From;
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatJoinRequest:
                    botUser = update.ChatJoinRequest.From;
                    break;
            }
            if (botUser == null)
            {
                return Result<Telegram.Bot.Types.User>.Fail("Не найден юзер");
            }
            return Result<Telegram.Bot.Types.User>.Done(botUser);
        }

        public Result<Tuple<StateUserEnum, string>> Handle(Update update)
        {
            if (update == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Странное сообщение");
            }

            var result = GetUser(update);
            Telegram.Bot.Types.User botUser = result ? result.Value : null;

            if (botUser == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера? Странно");
            }

            if (!_userService.TryGetUserById(botUser.Id, out User user))
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера? Странно");
            }

            if (_dictionary.TryGetValue(user.State, out Func<Update, Result<Tuple<StateUserEnum, string>>> action))
            {
                if (action == null)
                {
                    return Result<Tuple<StateUserEnum, string>>.Fail("Нет подходящего действия");
                }

                return action(update);
            }
            return Result<Tuple<StateUserEnum, string>>.Fail("Нет подходящего действия");
        }

        public Result<ReplyKeyboardMarkup> GetKeyboardMarkup(StateUserEnum stateUser, Update update)
        {
            if (!_dictionaryReplyKeyboardMarkup.TryGetValue(stateUser, out var generator) || generator == null)
            {
                return Result<ReplyKeyboardMarkup>.Fail("нет такого");
            }
            return generator.Invoke(update);
        }
    }
}