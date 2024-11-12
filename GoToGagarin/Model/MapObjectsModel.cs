using MapControlLib.Models;
using Newtonsoft.Json;

namespace GoToGagarin.Model;

public class MapObjectsModel : INavigateableModel
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("images")] public List<ImagesModel>? Images { get; set; }
    [JsonProperty("node")] public int? Node { get; set; }
    [JsonProperty("area")] public int? Area { get; set; }
}

public class Terminal : INavigateableModel
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("node")] public int? Node { get; set; }
    [JsonProperty("area")] public int? Area { get; set; }
}