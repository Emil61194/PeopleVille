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
        FileInfo file =  new(fullPath);
        if (file.Exists)
        {
            bool world = GameEngine.Initialize(fullPath);
            if (world) return true;
        }
        return false;
    }
}