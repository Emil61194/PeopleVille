using PeopleVille.Engine;
using Microsoft.AspNetCore.Hosting;

namespace PeopleVille.Server.Services;

public class GameService(GameEngine gameEngine)
{
    public readonly GameEngine GameEngine = gameEngine;
    public bool TryInitialize(string filename)
    {
        string saveFilesDirectory = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "PeopleVille.Core",
            "saves"
        ));
            
        string fullPath = saveFilesDirectory +  filename;
        FileInfo file =  new FileInfo(fullPath);
        if (file.Exists)
        {
            bool world = gameEngine.Initialize(fullPath);
            if (world) {
                _ = gameEngine.Run();
                return true;
            }
        }
        return false;
    }
    
    public bool TryInitializeNew()
    {
        bool world = gameEngine.Initialize();
        if (world)
        {
            _ = gameEngine.Run();
            return true;
        }
        return false;
    }
}