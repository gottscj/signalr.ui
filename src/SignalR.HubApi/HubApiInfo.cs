using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubApiInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }= string.Empty;
        
        [JsonProperty("version")]
        public string Version { get; set; }= string.Empty;
        
        [JsonProperty("title")]
        public string Title { get; set; }= string.Empty;

        [JsonProperty("contact")]
        public HubApiContact Contact { get; set; } = new HubApiContact();

        [JsonProperty("hubApiEndpoint")]
        public string HubApiEndpoint { get; set; } = "/hubapi";
    }
}