using Newtonsoft.Json;

namespace GoToGagarin.Model;

public class InactivityModel
{
    [JsonProperty("id")] public int id { get; set; }
    [JsonProperty("media")] public string media { get; set; }
    [JsonProperty("showTime")] public int showTime { get; set; }
}