using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("methods")]
        public IList<HubMethod> Methods { get; set; } = new List<HubMethod>();
        
        [JsonProperty("clientMethods")]
        public IList<HubMethod> ClientMethods { get; set; } = new List<HubMethod>();
    }
}