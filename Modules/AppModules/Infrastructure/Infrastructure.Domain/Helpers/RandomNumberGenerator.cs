using System;
using System.Text;

namespace Infrastructure.Domain.Helpers
{
    public static class RandomGenerator
    {
        private static readonly Random Random = new();

        public static Random GetRandom() => Random;

        /// <summary>
        /// Сгенерировать случайное число от 1 до 999999
        /// </summary>
        /// <returns></returns>
        public static int GenerateSixDigitRandomNumber()
        {
            int randomNumber = Random.Next(1, 1000000);
            return randomNumber;
        }

        /// <summary>
        /// Сгенерировать случайное число от 000001 до 999999 с ведущими нулями.
        /// </summary>
        /// <returns></returns>
        public static string GenerateFormattedSixDigitRandomNumber()
        {
            int number = GenerateSixDigitRandomNumber();
            return number.ToString("D6");
        }

        /// <summary>
        /// Создать строку из случайных символов из заданного набора символов (латинские буквы в верхнем регистре и цифры) указанной длины
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder builder = new(length);

            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[Random.Next(chars.Length)]);
            }

            return builder.ToString();
        }
    }
}