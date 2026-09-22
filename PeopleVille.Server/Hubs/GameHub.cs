using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Server.Services;

namespace PeopleVille.Server.Hubs;

[Authorize]
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
    public async Task<House> GetHouseData(string address)
    {
        return await Task.FromResult(gameService.GameEngine.GetHouseByAddress(address));
    }
    public async Task<Apartment> GetApartmentData(string address)
    {
        return await Task.FromResult(gameService.GameEngine.GetApartmentByAddress(address));
    }
    public async Task<Citizen> GetCitizenData(int id)
    {
        return await Task.FromResult(gameService.GameEngine.GetCitizenById(id));
    }
}