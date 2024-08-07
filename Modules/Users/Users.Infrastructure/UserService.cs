﻿using Common.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Users.Domain;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using Users.Infrastructure.Interfaces.Managers;
using User = Users.Domain.Models.User;

namespace Users.Infrastructure
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        public UserService(IUserDbManager userDbManager)
        {
            _userDbManager = userDbManager;
            _dbContext = _userDbManager.DbContext;
        }

        /// <inheritdoc />
        public User CreateUser(long userId, string name, string nick)
        {
            User user = new()
            {
                Id = userId,
                Name = name,
                Nick = nick,
                State = StateUserEnum.SetName,
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        /// <inheritdoc />
        public void DeleteUser(User question)
        {
            if (_dbContext.Users.Contains(question))
            {
                DeleteUser(question.Id);
            }
        }

        /// <inheritdoc />
        public void DeleteUser(long questionId)
        {
            User user = _dbContext.Users.Find(questionId);
            if (user == null)
            {
                return;
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public IList<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public bool TryGetUserById(long userId, out User user)
        {
            user = _dbContext.Users.Find(userId);
            return user != null;
        }

        /// <inheritdoc />
        public void UpdateUser(User user)
        {
            User oldUser = _dbContext.Users.Find(user.Id);
            if (oldUser == null)
            {
                return;
            }

            oldUser.Name = user.Name;
            oldUser.State = user.State;
            oldUser.Nick = user.Nick;

            // обновление других свойств
            _dbContext.SaveChanges();
        }

        public bool TryGetUserByNiсk(string nick, out User user)
        {
            user = _dbContext.Users.FirstOrDefault(x => x.Nick == nick);
            return user != null;
        }

        public Result<Tuple<StateUserEnum, string>> UpdateUsername(Update update)
        {
            Message message = update?.Message;

            if (message == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет сообщения");
            }

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (TryGetUserById(message.From.Id, out User user))
                {
                    user.Name = message.Text;
                    UpdateUser(user);
                    return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.MainMenu,
                        "Вы в главном меню"));
                }

                return Result<Tuple<StateUserEnum, string>>.Fail("Нет такого пользователя");
            }

            return Result<Tuple<StateUserEnum, string>>.Fail("Сообщение не текстовое");
        }

        private readonly IUserDbManager _userDbManager;
        private readonly UserDbContext _dbContext;
    }
}