using System;
using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubParameter : HubTypeInfo
    {
        public HubParameter(string name, Type type) : base(type)
        {
            Name = name;
        }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; }
    }
}