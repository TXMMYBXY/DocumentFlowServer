using DocumentFlowServer.Worker.Interface;
using TemplateEngine.Docx;

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
        foreach (var kv in fields)
        {
            content.Fields.Add(new FieldContent(kv.Key, kv.Value ?? string.Empty));
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

}
