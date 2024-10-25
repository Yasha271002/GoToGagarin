﻿using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace GoToGagarin.Model;

public class ImagesModel : ObservableObject
{
    [JsonProperty("image")] public string? Image { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
}