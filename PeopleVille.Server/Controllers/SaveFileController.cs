using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace PeopleVille.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveFileController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public SaveFileController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpGet("/get/savefile")]
        public async Task<IActionResult> GetSaveFileDataAsJson()
        {
            string saveFilePath = Path.GetFullPath(Path.Combine(
                environment.ContentRootPath,
                "..",
                "PeopleVille.Core",
                "saves",
                "test_save.json"));

            if (!System.IO.File.Exists(saveFilePath))
            {
                return NotFound("The test save file could not be found.");
            }

            await using FileStream saveFile = System.IO.File.OpenRead(saveFilePath);
            JsonNode? saveData = await JsonNode.ParseAsync(saveFile);

            if (saveData == null)
            {
                BadRequest("Unable to parse save data from JSON file");
            }

            return new JsonResult(saveData);
        }
    }
}