using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace WebApi.Models;

public partial class MapObjectsModel:ObservableObject
{
    [JsonProperty("id")] public int Id;
    [JsonProperty("title"), ObservableProperty] public string? _title;
    [JsonProperty("description"), ObservableProperty] public string? _description;
    [JsonProperty("images"), ObservableProperty] public List<ImagesModel>? _images;
    [JsonProperty("type"), ObservableProperty] public string? _iype;
    
    //[JsonProperty("node")]
    //public int Node { get; set; }

    //[JsonProperty("area")]
    //public int Area { get; set; }
}