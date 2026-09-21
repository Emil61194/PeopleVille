using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using PeopleVille.Server.Services;

namespace PeopleVille.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveFileController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly GameService gameService;

        public SaveFileController(IWebHostEnvironment environment, GameService gameService)
        {
            this.environment = environment;
            this.gameService = gameService;
        }

        private record SaveFileInfo(string filename, string modifyDate, string size);
        
        [HttpGet("/get/savefiles")]
        public async Task<IActionResult> GetSaveFileDataAsJson()
        {
            string saveFilesDirectory = Path.GetFullPath(Path.Combine(
                environment.ContentRootPath,
                "..",
                "PeopleVille.Core",
                "saves"
                ));
            
            
            if (!Directory.Exists(saveFilesDirectory))
            {
                return NotFound("Save Files Directory Not Found, attemped path: " + saveFilesDirectory);
            }
            string[] saveFiles = Directory.GetFiles(saveFilesDirectory);

            List<SaveFileInfo> saveFilesInfo = new List<SaveFileInfo>();

            foreach (string saveFile in saveFiles)
            {
                FileInfo saveFileInfo = new FileInfo(saveFile);
                saveFilesInfo.Add(new  SaveFileInfo(saveFileInfo.Name, saveFileInfo.LastWriteTime.ToString(), saveFileInfo.Length.ToString()));  
            }

            return new JsonResult(saveFilesInfo);
        }

        [HttpGet("/get/savefiles/{filename}")]
        public async Task<IActionResult> TrySaveFile(string filename)
        {
            string saveFilesDirectory = Path.GetFullPath(Path.Combine(
                environment.ContentRootPath,
                "..",
                "PeopleVille.Core",
                "saves"
                ));
            
            string fullPath = saveFilesDirectory +  filename;
            FileInfo file =  new FileInfo(fullPath);
            if (file.Exists)
            {
                bool valid = gameService.TryInitialize(fullPath);
                if (valid)
                {
                    return Ok();
                }
            }

            return NotFound();
        }
    }
}