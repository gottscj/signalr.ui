using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalR.HubApi
{
    public class HubTypeInfo
    {
        [JsonIgnore]
        public Type Type { get; }

        public HubTypeInfo(Type type)
        {
            var observable = false;
            if (typeof(Task).IsAssignableFrom(type) && type.IsGenericType)
            {
                Type = new HubTypeInfo(type.GenericTypeArguments.Single()).Type;
            }
            else if (IsStreaming(type))
            {
                Type = type.GenericTypeArguments.Single();
                observable = true;
            }
            else
            {
                Type = type; 
            }
            
            var schemaType = JsonTypeHelper.GetSchemaTypeString(Type);
            Schema = new Dictionary<string, object>
            {
                ["type"] = schemaType,
                ["observable"] = observable
            };

            if (schemaType == "object")
            {
                Schema["$ref"] = $"#/definitions/{Type.Name}";
            }
            else if (schemaType == "array")
            {

                var arrayType = Type.IsArray ? Type.GetElementType() : Type.GetGenericArguments().Single();
                if (arrayType == null)
                {
                    throw new InvalidOperationException($"Could not get element type for given type: {type}");
                }

                Schema["items"] =
                    new Dictionary<string, object>
                    {
                        ["$ref"] = $"#/definitions/{arrayType.Name}"
                    };
            }
        }

        [JsonProperty("required")]
        public bool Required { get; } = true;

        [JsonProperty("schema", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Schema { get; set; }

        private bool IsStreaming(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypeArg = type.GenericTypeArguments.First();
            var channelReaderType = typeof(ChannelReader<>).MakeGenericType(genericTypeArg);
            var asyncEnumerableType = typeof(IAsyncEnumerable<>).MakeGenericType(genericTypeArg);
            return channelReaderType.IsAssignableFrom(type) || asyncEnumerableType.IsAssignableFrom(type);
        }
    }
}