using Microsoft.AspNetCore.SignalR;
using PeopleVille.Engine;
using PeopleVille.Server.Hubs;

namespace PeopleVille.Server.Infrastructure;

    
public class SignalREventPublisher(IHubContext<GameHub> hubContext) : IEventPublisher
{
    public Task PublishEventAsync(object EventData)
    {
        return hubContext.Clients
            .All
            .SendAsync("Event", EventData);
    }
}