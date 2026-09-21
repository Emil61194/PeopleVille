using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleVille.Server.Services;

namespace PeopleVille.Server.Hubs;

[Authorize]
public class GameHub(GameService gameService, SaveService saveService) : Hub
{
    public async Task<string> SaveGame()
    {
        if (!gameService.GameEngine.Ready || gameService.GameEngine.CurrentWorld is null)
        {
            throw new HubException("The game is not initialized.");
        }

        return await saveService.SaveAsync(gameService.GameEngine.CurrentWorld);
    }

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
