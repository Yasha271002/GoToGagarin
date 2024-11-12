using MapControlLib.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace GoToGagarin.Model;

public class MapObjectsModel : INavigateableModel
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("images")] public List<ImagesModel> Images { get; set; } = [];
    [JsonProperty("node")] public int? Node { get; set; }
    [JsonProperty("area")] public int? Area { get; set; }
    [JsonProperty("mapObjects")] public List<MapObjectsModel> MapObjects { get; set; } = [];
}

public class Terminal : INavigateableModel
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("node")] public int? Node { get; set; }
    [JsonProperty("area")] public int? Area { get; set; }
}

public record Paragraph(string Title, string Description);

public class MapObject(
    string title,
    List<ImagesModel> images,
    List<Paragraph> paragraphs,
    int? node,
    int? area) : INavigateableModel
{
    public string Title { get; set; } = title;
    public int? Node { get; set; } = node;
    public int? Area { get; set; } = area;

    public List<ImagesModel> Images { get; set; } = images;
    public List<Paragraph> Paragraphs { get; set; } = paragraphs;
}

public static class ParagraphExtensions
{
    public static Paragraph FilterText(this Paragraph paragraph)=>paragraph with
    {
        Description = Regex.Replace(paragraph.Description, @"[^а-яА-Я0-9\s.,:!?'\-]", "")
    };
}

public static class MapObjectMappingExtension
{
    public static List<MapObject> ToMapObjects(this IEnumerable<MapObjectsModel> mapObjects) =>
        mapObjects
            .Select(mo => mo.ToMapObject())
            .ToList();

    public static MapObject ToMapObject(this MapObjectsModel mapObject) => mapObject.Type switch
    {
        "object"=>FromMapObject(mapObject),
        "complex"=>FromMapComplex(mapObject),
        _ => throw new ArgumentOutOfRangeException()
    };

    private static MapObject FromMapObject(MapObjectsModel mapObject)
        => new(
            mapObject.Title, 
            mapObject.Images, 
            [new Paragraph(mapObject.Title, mapObject.Description).FilterText()],
            mapObject.Node, 
            mapObject.Area);

    private static MapObject FromMapComplex(MapObjectsModel mapObject)
        => new(
            mapObject.Title,
            mapObject.MapObjects.SelectMany(mo => mo.Images).ToList(),
            mapObject.MapObjects.Select(mo => new Paragraph(mo.Title, mo.Description).FilterText()).ToList(),
            mapObject.Node,
            mapObject.Area);

}