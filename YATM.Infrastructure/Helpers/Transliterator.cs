using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATM.Infrastructure.Helpers
{
    public class Transliterator
    {
        private static readonly Dictionary<char, string> _translitMap = new Dictionary<char, string>
        {
            { 'А', "A" }, { 'Б', "B" }, { 'В', "V" }, { 'Г', "G" }, { 'Д', "D" },
            { 'Е', "E" }, { 'Ё', "E" }, { 'Ж', "Zh" }, { 'З', "Z" }, { 'И', "I" },
            { 'Й', "Y" }, { 'К', "K" }, { 'Л', "L" }, { 'М', "M" }, { 'Н', "N" },
            { 'О', "O" }, { 'П', "P" }, { 'Р', "R" }, { 'С', "S" }, { 'Т', "T" },
            { 'У', "U" }, { 'Ф', "F" }, { 'Х', "Kh" }, { 'Ц', "Ts" }, { 'Ч', "Ch" },
            { 'Ш', "Sh" }, { 'Щ', "Shch" }, { 'Ы', "Y" }, { 'Э', "E" }, { 'Ю', "Yu" },
            { 'Я', "Ya" }, { 'Ь', "" }, { 'Ъ', "" }, { 'а', "a" }, { 'б', "b" },
            { 'в', "v" }, { 'г', "g" }, { 'д', "d" }, { 'е', "e" }, { 'ё', "e" },
            { 'ж', "zh" }, { 'з', "z" }, { 'и', "i" }, { 'й', "y" }, { 'к', "k" },
            { 'л', "l" }, { 'м', "m" }, { 'н', "n" }, { 'о', "o" }, { 'п', "p" },
            { 'р', "r" }, { 'с', "s" }, { 'т', "t" }, { 'у', "u" }, { 'ф', "f" },
            { 'х', "kh" }, { 'ц', "ts" }, { 'ч', "ch" }, { 'ш', "sh" }, { 'щ', "shch" },
            { 'ы', "y" }, { 'э', "e" }, { 'ю', "yu" }, { 'я', "ya" }
        };

        public static string Transliterate(string input)
        {
            return string.Concat(input.Select(c => _translitMap.ContainsKey(c) ? _translitMap[c] : c.ToString()));
        }
    }
}
