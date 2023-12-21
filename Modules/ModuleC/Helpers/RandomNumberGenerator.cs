using System;

namespace TelegramAPI.Test.Helpers
{
    public static class RandomNumberGenerator
    {
        /// <summary>
        /// Сгенерировать случайное число от 1 до 999999
        /// </summary>
        /// <returns></returns>
        public static int GenerateSixDigitRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 1000000);
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
    }
}