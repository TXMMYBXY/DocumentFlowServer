using System;
using System.Collections.Generic;
using System.Linq;
using NPetrovich;
using DocumentFlowServer.Worker.Interface;
using TemplateEngine.Docx;
using System.Globalization;

namespace DocumentFlowServer.Worker.Service;

public class DocumentTemplateService : IDocumentTemplateService
{
    /// <summary>
    /// Заполняет Word-шаблон значениями из словаря fields.
    /// Использует TemplateEngine.Docx для корректной работы с Run, таблицами и другими элементами.
    /// </summary>
    /// <param name="templatePath">Путь к исходному шаблону</param>
    /// <param name="fields">Словарь <ключ, значение> для замены</param>
    /// <returns>Массив байт заполненного документа</returns>
    public async Task<byte[]> FillTemplateAsync(string templatePath, Dictionary<string, string> fields)
    {
        if (string.IsNullOrWhiteSpace(templatePath))
            throw new ArgumentException("Template path is null or empty", nameof(templatePath));

        // Убираем невидимые символы и обрезаем пробелы
        templatePath = templatePath
            .Trim()
            .Replace("\u202A", "") // Left-to-Right Embedding
            .Replace("\u202B", "") // Right-to-Left Embedding
            .Replace("\u202C", "") // Pop Directional Formatting
            .Replace("\u200E", "") // Left-to-Right Mark
            .Replace("\u200F", "") // Right-to-Left Mark
            .Replace("\uFEFF", ""); // BOM

        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Template file not found", templatePath);

        // Загружаем шаблон в память
        await using var mem = new MemoryStream();
        await using (var original = File.OpenRead(templatePath))
        {
            await original.CopyToAsync(mem);
        }
        mem.Position = 0;

        // Создаем Content для TemplateEngine.Docx
        var content = new Content();

        // Поддерживаемые коды падежей, которые могут быть указаны после ':' в теге
        var supportedCases = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "rod", // родительный
            "dat", // дательный
            "acc", // винительный (acc/ vin иногда используются)
            "vin",
            "ins", // творительный
            "gen", // (синоним родительного)
            "prep", // предложный
            "loc"  // локатив (реже)
        };

        var supportedDateFormats = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["date"] = "dd.MM.yyyy",
            ["date_short"] = "dd.MM.yy",
            ["date_iso"] = "yyyy-MM-dd",
            ["date_ru"] = "d MMMM yyyy",
            ["date_ru_full"] = "«dd» MMMM yyyy г."
        };

        foreach (var kv in fields)
        {
            var key = kv.Key ?? string.Empty;
            var value = kv.Value ?? string.Empty;

            // Если в ключе есть ':' - после него может идти код падежа, например "recipient:rod"
            var fieldName = key;
            string finalValue = value;

            var idx = key.IndexOf(':');

            if (idx >= 0 && idx < key.Length - 1)
            {
                var suffix = key[(idx + 1)..].Trim();

                // Падежи
                if (supportedCases.Contains(suffix))
                {
                    try
                    {
                        finalValue = TryDeclineFullName(value, suffix) ?? value;
                    }
                    catch
                    {
                        finalValue = value;
                    }
                }
                // Даты
                else if (supportedDateFormats.TryGetValue(
                    suffix,
                    out var format))
                {
                    try
                    {
                        if (DateTime.TryParse(value, out var date))
                        {
                            finalValue = date.ToString(
                                format,
                                new CultureInfo("ru-RU"));
                        }
                    }
                    catch
                    {
                        finalValue = value;
                    }
                }
            }

            content.Fields.Add(new FieldContent(fieldName, finalValue));
        }

        // Заполняем шаблон
        using (var template = new TemplateProcessor(mem)
            .SetRemoveContentControls(true)) // удаляем управляющие поля
        {
            template.FillContent(content);
            template.SaveChanges();
        }

        return mem.ToArray();
    }

    // Попытка просклонять ФИО в указанный падеж через NPetrovich (прямой вызов).
    private static string? TryDeclineFullName(string fullName, string caseCode)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return fullName;

        // Разделяем ФИО на части: обычно Фамилия Имя Отчество
        var parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var surname = parts.Length >= 1 ? parts[0] : string.Empty;
        var name = parts.Length >= 2 ? parts[1] : string.Empty;
        var patronymic = parts.Length >= 3 ? string.Join(" ", parts.Skip(2)) : string.Empty;

        // Сопоставление кодов падежей с enum Case библиотеки NPetrovich
        Case targetCase = caseCode?.ToLowerInvariant() switch
        {
            "rod" or "gen" => Case.Genitive,
            "dat" => Case.Dative,
            "vin" or "acc" => Case.Accusative,
            "ins" => Case.Instrumental,
            "prep" => Case.Prepositional,
            "loc" => Case.Prepositional,
            _ => Case.Genitive
        };

        try
        {
            var pet = new Petrovich()
            {
                FirstName = name,
                LastName = surname,
                MiddleName = patronymic,
                AutoDetectGender = true
            };

            var inflected = pet.InflectTo(targetCase);

            var outSurname = string.IsNullOrWhiteSpace(inflected.LastName) ? surname : inflected.LastName;
            var outName = string.IsNullOrWhiteSpace(inflected.FirstName) ? name : inflected.FirstName;
            var outPatr = string.IsNullOrWhiteSpace(inflected.MiddleName) ? patronymic : inflected.MiddleName;

            return string.IsNullOrEmpty(outPatr)
                ? $"{outSurname} {outName}".Trim()
                : $"{outSurname} {outName} {outPatr}".Trim();
        }
        catch
        {
            // в случае ошибки возвращаем исходную строку
            return fullName;
        }
    }

    // helpers from previous reflection-based approach removed; using direct NPetrovich API instead

}
