using Common.Core.Components;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using User = Users.Domain.Models.User;

namespace Users.Infrastructure
{
    public class TelegramHandlerService : ITelegramHandlerService
    {
        Dictionary<StateUserEnum, Func<Message, Result<StateUserEnum>>> _dictionary;
        private readonly IUserService _userService;

        public TelegramHandlerService(IUserService userService)
        {
            _dictionary = new Dictionary<StateUserEnum, Func<Message, Result<StateUserEnum>>>();
            _userService = userService;
        }

        public ITelegramHandlerService RegisterHandler(StateUserEnum stateUser, Func<Message, Result<StateUserEnum>> handler)
        {
            _dictionary.Add(stateUser, handler);
            return this;
        }

        public Result<StateUserEnum> Handle(Message message)
        {
            if (message == null)
            {
                return Result<StateUserEnum>.Fail("Странное сообщение");
            }

            Telegram.Bot.Types.User botUser = message.From;

            if (botUser == null)
            {
                return Result<StateUserEnum>.Fail("Нет юзера? Странно");
            }

            if (!_userService.TryGetUserById(botUser.Id, out User user))
            {
                return Result<StateUserEnum>.Fail("Нет юзера? Странно");
            }

            if (_dictionary.TryGetValue(user.State, out Func<Message, Result<StateUserEnum>> action))
            {
                if (action == null)
                {
                    return Result<StateUserEnum>.Fail("Нет подходящего действия");
                }

                return action(message);
            }
            return Result<StateUserEnum>.Fail("Нет подходящего действия");
        }
    }
}