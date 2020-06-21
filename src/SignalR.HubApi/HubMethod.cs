using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubMethod
    {
        public HubMethod(MethodInfo methodInfo, IEnumerable<ParameterInfo> parameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            Name = methodInfo.Name;

            Response = new HubResponse(methodInfo.ReturnType);
            Parameters = parameters
                .Select(p => new HubParameter(p.Name, p.ParameterType)).ToList();
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("response")]
        public HubResponse Response { get; }
        
        [JsonProperty("params")]
        public IList<HubParameter> Parameters { get; }
    }
}