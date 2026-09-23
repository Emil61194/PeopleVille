using PeopleVille.Engine;
using Microsoft.AspNetCore.Hosting;

namespace PeopleVille.Server.Services;

public class GameService
{
    public readonly GameEngine GameEngine;

    public GameService(GameEngine gameEngine)
    {
        GameEngine = gameEngine;
    }

    public bool TryInitialize(string filename)
    {
        string saveFilesDirectory = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "PeopleVille.Core",
            "saves"
        ));
            
        string fullPath = saveFilesDirectory +  filename;
        FileInfo file =  new(fullPath);
        if (file.Exists)
        {
            bool world = GameEngine.Initialize(fullPath);
            if (world) {
                _ = GameEngine.Run();
                return true;
            }
        }
        return false;
    }
    
    public bool TryInitializeNew()
    {
        bool world = GameEngine.Initialize();
        if (world)
        {
            _ = GameEngine.Run();
            return true;
        }
        return false;
    }
}