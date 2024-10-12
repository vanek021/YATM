using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YATM.Infrastructure.Helpers;

namespace YATM.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitials(this string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "NN"; // В случае пустого имени возвращаем "NN" как стандартное

            var nameParts = fullName.Trim().Split(' ');

            // Берем первую букву от имени и фамилии
            string initials = string.Concat(
                nameParts[0][0].ToString().ToUpper(),
                nameParts.Length > 1 ? nameParts[1][0].ToString().ToUpper() : string.Empty
            );

            return initials;
        }

        public static string GetUpperFirstThreeCharacters(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty; // Если строка пуста, вернуть пустую строку
            }

            // Проверяем, есть ли русские символы
            if (Regex.IsMatch(input, @"\p{IsCyrillic}"))
            {
                // Если есть, транслитерируем строку
                input = Transliterator.Transliterate(input);
            }

            // Извлекаем первые 3 символа
            return input.Length > 3 ? input.Substring(0, 3).ToUpper() : input.ToUpper();
        }
    }
}
