using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Server.Services;

namespace PeopleVille.Server.Hubs;

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
    public async Task<List<object>> GetAllHomes()
    {
        return await Task.FromResult(gameService.GameEngine.GetAllHomes());
    }
    public async Task<List<Citizen>> GetAllCitizens()
    {
        return await Task.FromResult(gameService.GameEngine.GetAllCitizens());
    }
    public async Task<List<ShoppingCenter>> GetAllWorkplaces()
    {
        return await Task.FromResult(gameService.GameEngine.GetAllWorkplaces());
    }
}