using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace YATM.Services.Recipes.Import
{
    public static class RecipeImportParserHelpers
    {
        public static string NormalizeHost(string host)
        {
            var normalized = host.Trim().ToLowerInvariant();
            if (normalized.StartsWith("www."))
                normalized = normalized[4..];
            return normalized;
        }

        public static bool HostMatches(Uri uri, IReadOnlyCollection<string> supportedHosts)
        {
            var host = NormalizeHost(uri.Host);
            return supportedHosts.Any(h => NormalizeHost(h) == host);
        }

        public static string? GetFirstMatchGroup(string value, string pattern, int group = 1)
        {
            var match = Regex.Match(value, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!match.Success || match.Groups.Count <= group)
                return null;

            return match.Groups[group].Value;
        }

        public static List<string> GetAllMatches(string value, string pattern, int group = 1)
        {
            var matches = Regex.Matches(value, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var result = new List<string>();

            foreach (Match match in matches)
            {
                if (!match.Success || match.Groups.Count <= group)
                    continue;

                var cleaned = CleanText(match.Groups[group].Value);
                if (!string.IsNullOrWhiteSpace(cleaned))
                    result.Add(cleaned);
            }

            return result;
        }

        public static string CleanText(string rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return string.Empty;

            var withNewLines = Regex.Replace(rawHtml, "<br\\s*/?>", "\n", RegexOptions.IgnoreCase);
            var withoutTags = Regex.Replace(withNewLines, "<[^>]+>", " ");
            var decoded = WebUtility.HtmlDecode(withoutTags);

            var lines = decoded
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => Regex.Replace(line, "\\s+", " ").Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();

            return string.Join("\n", lines);
        }

        public static string BuildContentHtml(string? description, IEnumerable<string> ingredients, IEnumerable<string> steps)
        {
            var ingredientList = ingredients
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Distinct()
                .ToList();

            var stepList = steps
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Distinct()
                .ToList();

            var descriptionValue = description?.Trim();
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(descriptionValue))
            {
                builder.Append("<p>");
                builder.Append(WebUtility.HtmlEncode(descriptionValue));
                builder.Append("</p>");
            }

            if (ingredientList.Count > 0)
            {
                builder.Append("<h3>Ингредиенты</h3><ul>");
                foreach (var ingredient in ingredientList)
                {
                    builder.Append("<li>");
                    builder.Append(WebUtility.HtmlEncode(ingredient));
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
            }

            if (stepList.Count > 0)
            {
                builder.Append("<h3>Шаги приготовления</h3><ol>");
                foreach (var step in stepList)
                {
                    builder.Append("<li>");
                    builder.Append(WebUtility.HtmlEncode(step));
                    builder.Append("</li>");
                }
                builder.Append("</ol>");
            }

            return builder.ToString();
        }

        public static string DecodeWindows1251(byte[] contentBytes)
        {
            var chars = new char[contentBytes.Length];

            for (int i = 0; i < contentBytes.Length; i++)
            {
                var b = contentBytes[i];
                chars[i] = b < 0x80 ? (char)b : Windows1251Map[b - 0x80];
            }

            return new string(chars);
        }

        // Mapping table for bytes 0x80..0xFF.
        private static readonly char[] Windows1251Map =
        {
            '\u0402', '\u0403', '\u201A', '\u0453', '\u201E', '\u2026', '\u2020', '\u2021',
            '\u20AC', '\u2030', '\u0409', '\u2039', '\u040A', '\u040C', '\u040B', '\u040F',
            '\u0452', '\u2018', '\u2019', '\u201C', '\u201D', '\u2022', '\u2013', '\u2014',
            '\uFFFD', '\u2122', '\u0459', '\u203A', '\u045A', '\u045C', '\u045B', '\u045F',
            '\u00A0', '\u040E', '\u045E', '\u0408', '\u00A4', '\u0490', '\u00A6', '\u00A7',
            '\u0401', '\u00A9', '\u0404', '\u00AB', '\u00AC', '\u00AD', '\u00AE', '\u0407',
            '\u00B0', '\u00B1', '\u0406', '\u0456', '\u0491', '\u00B5', '\u00B6', '\u00B7',
            '\u0451', '\u2116', '\u0454', '\u00BB', '\u0458', '\u0405', '\u0455', '\u0457',
            '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415', '\u0416', '\u0417',
            '\u0418', '\u0419', '\u041A', '\u041B', '\u041C', '\u041D', '\u041E', '\u041F',
            '\u0420', '\u0421', '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427',
            '\u0428', '\u0429', '\u042A', '\u042B', '\u042C', '\u042D', '\u042E', '\u042F',
            '\u0430', '\u0431', '\u0432', '\u0433', '\u0434', '\u0435', '\u0436', '\u0437',
            '\u0438', '\u0439', '\u043A', '\u043B', '\u043C', '\u043D', '\u043E', '\u043F',
            '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445', '\u0446', '\u0447',
            '\u0448', '\u0449', '\u044A', '\u044B', '\u044C', '\u044D', '\u044E', '\u044F',
        };
    }
}
