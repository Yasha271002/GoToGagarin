using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace WebApi.Models;

public class ImagesModel:ObservableObject
{
    [JsonProperty("image")] public string? _image;
    [JsonProperty("title")] public string? _title;
}