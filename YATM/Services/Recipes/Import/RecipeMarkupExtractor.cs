using System.Net;
using System.Text.Json;
using HtmlAgilityPack;

namespace YATM.Services.Recipes.Import
{
    internal static class RecipeMarkupExtractor
    {
        public static RecipeImportPayload? Extract(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return null;

            var document = new HtmlDocument();
            document.LoadHtml(html);

            return ExtractFromJsonLd(document) ?? ExtractFromMicrodata(document);
        }

        private static RecipeImportPayload? ExtractFromJsonLd(HtmlDocument document)
        {
            var jsonNodes = document.DocumentNode.SelectNodes("//script[@type='application/ld+json']");
            if (jsonNodes == null || jsonNodes.Count == 0)
                return null;

            foreach (var node in jsonNodes)
            {
                var rawJson = WebUtility.HtmlDecode(node.InnerText)?.Trim();
                if (string.IsNullOrWhiteSpace(rawJson))
                    continue;

                try
                {
                    using var jsonDoc = JsonDocument.Parse(rawJson);
                    var payload = ExtractFromJsonElement(jsonDoc.RootElement);
                    if (payload != null)
                        return payload;
                }
                catch (JsonException)
                {
                    // Some pages contain malformed JSON-LD blocks.
                }
            }

            return null;
        }

        private static RecipeImportPayload? ExtractFromJsonElement(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                if (IsRecipeNode(element))
                {
                    var payload = BuildPayloadFromRecipeNode(element);
                    if (payload != null)
                        return payload;
                }

                foreach (var property in element.EnumerateObject())
                {
                    var nested = ExtractFromJsonElement(property.Value);
                    if (nested != null)
                        return nested;
                }

                return null;
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    var nested = ExtractFromJsonElement(item);
                    if (nested != null)
                        return nested;
                }
            }

            return null;
        }

        private static bool IsRecipeNode(JsonElement element)
        {
            if (!TryGetPropertyIgnoreCase(element, "@type", out var typeElement))
                return false;

            if (typeElement.ValueKind == JsonValueKind.String)
                return IsRecipeTypeValue(typeElement.GetString());

            if (typeElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var value in typeElement.EnumerateArray())
                {
                    if (value.ValueKind == JsonValueKind.String && IsRecipeTypeValue(value.GetString()))
                        return true;
                }
            }

            return false;
        }

        private static bool IsRecipeTypeValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.Contains("Recipe", StringComparison.OrdinalIgnoreCase);
        }

        private static RecipeImportPayload? BuildPayloadFromRecipeNode(JsonElement element)
        {
            var title = GetStringProperty(element, "name") ?? GetStringProperty(element, "headline");
            if (string.IsNullOrWhiteSpace(title))
                return null;

            var description = GetStringProperty(element, "description");
            var ingredients = GetStringCollectionProperty(element, "recipeIngredient");

            var steps = new List<string>();
            if (TryGetPropertyIgnoreCase(element, "recipeInstructions", out var instructionsElement))
                ExtractInstructionItems(instructionsElement, steps);

            return new RecipeImportPayload(title, description, ingredients, steps);
        }

        private static List<string> GetStringCollectionProperty(JsonElement element, string propertyName)
        {
            var result = new List<string>();
            if (!TryGetPropertyIgnoreCase(element, propertyName, out var propertyElement))
                return result;

            ExtractTextValues(propertyElement, result);
            return result;
        }

        private static void ExtractInstructionItems(JsonElement instructionsElement, List<string> result)
        {
            if (instructionsElement.ValueKind == JsonValueKind.Object
                && TryGetPropertyIgnoreCase(instructionsElement, "itemListElement", out var itemListElement))
            {
                ExtractInstructionItems(itemListElement, result);
                return;
            }

            if (instructionsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in instructionsElement.EnumerateArray())
                    ExtractInstructionItems(item, result);

                return;
            }

            if (instructionsElement.ValueKind == JsonValueKind.Object)
            {
                if (TryGetPropertyIgnoreCase(instructionsElement, "text", out var textElement))
                {
                    ExtractTextValues(textElement, result);
                    return;
                }

                if (TryGetPropertyIgnoreCase(instructionsElement, "name", out var nameElement))
                {
                    ExtractTextValues(nameElement, result);
                    return;
                }
            }

            ExtractTextValues(instructionsElement, result);
        }

        private static void ExtractTextValues(JsonElement element, List<string> result)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    var normalizedText = NormalizeText(element.GetString());
                    if (!string.IsNullOrWhiteSpace(normalizedText))
                        result.Add(normalizedText);
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                        ExtractTextValues(item, result);
                    break;

                case JsonValueKind.Object:
                    if (TryGetPropertyIgnoreCase(element, "text", out var textElement))
                    {
                        ExtractTextValues(textElement, result);
                    }
                    else if (TryGetPropertyIgnoreCase(element, "name", out var nameElement))
                    {
                        ExtractTextValues(nameElement, result);
                    }
                    else if (TryGetPropertyIgnoreCase(element, "itemListElement", out var listElement))
                    {
                        ExtractTextValues(listElement, result);
                    }
                    break;
            }
        }

        private static string? GetStringProperty(JsonElement element, string propertyName)
        {
            if (!TryGetPropertyIgnoreCase(element, propertyName, out var propertyElement))
                return null;

            return propertyElement.ValueKind == JsonValueKind.String
                ? NormalizeText(propertyElement.GetString())
                : null;
        }

        private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        private static RecipeImportPayload? ExtractFromMicrodata(HtmlDocument document)
        {
            var title = NormalizeText(document.DocumentNode.SelectSingleNode("//*[@itemprop='name' or @itemProp='name']")?.InnerText)
                        ?? NormalizeText(document.DocumentNode.SelectSingleNode("//h1")?.InnerText);

            if (string.IsNullOrWhiteSpace(title))
                return null;

            var description = NormalizeText(document.DocumentNode.SelectSingleNode("//*[@itemprop='description' or @itemProp='description']")?.InnerText);

            var ingredients = new List<string>();
            var ingredientNodes = document.DocumentNode.SelectNodes("//*[@itemprop='recipeIngredient' or @itemProp='recipeIngredient']");
            if (ingredientNodes != null)
            {
                foreach (var node in ingredientNodes)
                {
                    var ingredient = NormalizeText(node.InnerText);
                    if (!string.IsNullOrWhiteSpace(ingredient))
                        ingredients.Add(ingredient);
                }
            }

            var steps = new List<string>();
            var instructionNodes = document.DocumentNode.SelectNodes("//*[@itemprop='recipeInstructions' or @itemProp='recipeInstructions']");
            if (instructionNodes != null)
            {
                foreach (var node in instructionNodes)
                {
                    var stepParts = node.SelectNodes(".//*[@itemprop='text' or @itemProp='text']|.//li|.//p");
                    if (stepParts != null && stepParts.Count > 0)
                    {
                        foreach (var stepNode in stepParts)
                        {
                            var stepText = NormalizeText(stepNode.InnerText);
                            if (!string.IsNullOrWhiteSpace(stepText))
                                steps.Add(stepText);
                        }
                    }
                    else
                    {
                        var stepText = NormalizeText(node.InnerText);
                        if (!string.IsNullOrWhiteSpace(stepText))
                            steps.Add(stepText);
                    }
                }
            }

            return new RecipeImportPayload(title, description, ingredients, steps);
        }

        private static string? NormalizeText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var decoded = WebUtility.HtmlDecode(value)
                .Replace('\u00A0', ' ')
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ");

            var compact = string.Join(" ", decoded.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return string.IsNullOrWhiteSpace(compact) ? null : compact.Trim();
        }
    }
}
