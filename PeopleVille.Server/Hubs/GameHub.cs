using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleVille.Server.Services;

namespace PeopleVille.Server.Hubs;

public class GameHub(GameService gameService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (gameService.GameEngine.Ready)
        {
            await Clients.Caller.SendAsync("GameReady");
        }
        else
        {
            await Clients.Caller.SendAsync("GameNotReady");
            Context.Abort();
        }

        await base.OnConnectedAsync();
    }
}
