using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.FieldExtractor.Dtos;

public class FieldLocationDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("paragraphIndex")]
    public int? ParagraphIndex { get; set; }

    [JsonPropertyName("tableIndex")]
    public int? TableIndex { get; set; }

    [JsonPropertyName("rowIndex")]
    public int? RowIndex { get; set; }

    [JsonPropertyName("columnIndex")]
    public int? ColumnIndex { get; set; }
}