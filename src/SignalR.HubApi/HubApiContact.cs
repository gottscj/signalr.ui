using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubApiContact
    {
        [JsonProperty("name")] 
        public string Name { get; set; } = string.Empty;
        [JsonProperty("email")] 
        public string Email { get; set; } = string.Empty;
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }
}