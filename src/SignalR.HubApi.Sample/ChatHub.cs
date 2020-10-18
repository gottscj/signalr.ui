using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SignalR.HubApi.Sample
{
    public class ChatHub : Hub<IChatClient>
    {
        /// <summary>
        /// Sends a message to the chat
        /// </summary>
        public Task Send(string message)
        {
            if (message == string.Empty)
            {
                return Clients.All.Welcome();
            }

            return Clients.All.Send(message);
        }

        /// <summary>
        /// Echoes a message back to the sender
        /// </summary>
        public string Echo(string message)
        {
            return message;
        }

        /// <summary>
        /// Adds a person to the chat
        /// </summary>
        public Task AddPerson(Person person)
        {
            return Task.CompletedTask;
        }

        public ChannelReader<Event> GetEvents()
        {
            var channel = Channel.CreateUnbounded<Event>();
            
            // TODO: Write events
            return channel.Reader;
        }
    }
    
    public class Event
    {
        public string Type { get; set; }
    }

    public class Person
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    public interface IChatClient
    {
        /// <summary>
        /// Welcomes a person to the chat
        /// </summary>
        Task Welcome();

        /// <summary>
        /// Sends a message to the person
        /// </summary>
        Task Send(string message);
    }
}