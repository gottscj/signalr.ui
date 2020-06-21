using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Namotion.Reflection;
using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubApiConfiguration
    {
        [JsonProperty("info")]
        public HubApiInfo ApiInfo { get; set; } = new HubApiInfo();

        [JsonProperty("hubs")] public IReadOnlyList<HubModel> Hubs => _hubs;

        private readonly List<HubModel> _hubs = new List<HubModel>();
        
        public void AddHubDocumentation<THub>(string path) where THub : Hub
        {
            var description = typeof(THub).GetXmlDocsSummary();
            var hubModel = new HubModel
            {
                Name = typeof(THub).Name,
                Path = path,
                Description = description ?? string.Empty
            };
            CreateMethodInfo(typeof(THub), hubModel.Methods.Add);

            if (typeof(THub).BaseType?.IsGenericType == true)
            {
                var clientType = typeof(THub).BaseType.GenericTypeArguments.Single();
                CreateMethodInfo(clientType, hubModel.ClientMethods.Add);
            }
            
            _hubs.Add(hubModel);
        }

        private void CreateMethodInfo(Type type, Action<HubMethod> callback)
        {
            var clientMethodMetaData = type
                .GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly)
                .ToList();
            
            foreach (var methodInfo in clientMethodMetaData)
            {
                var parameters = methodInfo.GetParameters();
                var description = methodInfo.GetXmlDocsSummary();
                callback.Invoke(new HubMethod(methodInfo, parameters)
                {
                    Name = methodInfo.Name,
                    Description = description ?? string.Empty
                });
            }
        }
    }
}