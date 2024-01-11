﻿using System.Collections.Generic;
using Common.Extensions;
using DataDomain.Rooms.Rounds.Enums;
using ReactiveUI;

namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Раунд
    /// </summary>
    public class RoundModel : ReactiveObject
    {
        public RoundModel(RoundsLevelEnum level)
        {
            Level = level;
        }

        /// <summary>
        /// Номер раунда
        /// </summary>
        public RoundsLevelEnum Level
        {
            get => _level;
            init => this.RaiseAndSetIfChanged(ref _level, value);
        }

        /// <summary>
        /// Имя раунда. 1, 2, 3 или Финальный раунд
        /// </summary>
        public string Name => Level.GetDescription();

        /// <summary>
        /// Тема
        /// </summary>
        public string Theme
        {
            get => _theme;
            init => this.RaiseAndSetIfChanged(ref _theme, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TopicModel>? Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        private RoundsLevelEnum _level;
        private string _theme;
        private List<TopicModel>? _topics;

        /*/// <summary>
        /// Получить баллы за ответ на вопрос
        /// </summary>
        /// <param name="question">Вопрос</param>
        /// <param name="isRightAnswer">Это правильный ответ</param>
        /// <returns></returns>
        public int GetQuestionPoint(Question question, bool isRightAnswer)
        {
            int quality = isRightAnswer ? 1 : -1;
            if (Questions.Contains(question))
            {
                return question.Point * (int) Level * quality;
            }

            return 0;
        }*/
    }
}