using PeopleVille.Engine;

namespace PeopleVille.Server.Services;

public class GameService(GameEngine gameEngine)
{
    public readonly GameEngine GameEngine = gameEngine;
    public bool TryInitialize(string file)
    {
        bool world = gameEngine.Initialize(file);
        if (world) return true;
        return false;
    }
}