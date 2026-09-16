using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using PeopleVille.Core.Models;

namespace PeopleVille.Engine
{
    public class GameEngine
    {
        private int? currentTime;
        public bool doPause = false;
        public void Run(string filePath = "")
        {
            World world = CheckSave(filePath);

            while (true)
            {
                // Tid, (wait e.g 1 second) CHECK
                // Citizens do something ( With Events - Delegete)
                //// Check if citizen is adult, then work if person a job 



                // Random stuff happening ( e.g heatstroke, lack of supplies in town = death, virus ) 

                // When Ciitzens home at eating house, reduce private home inventory

                // Wait 1 second
                world.Time++;
                Thread.Sleep(1000);
                // if pause, 
                while (doPause)
                {
                    // Check for user input to resume or exit
                    //
                }
            }

        }

        public World CheckSave(string filePath)
        {
            if (File.Exists(filePath))
            {
                World? save = JsonSerializer.Deserialize<World>(File.ReadAllText(filePath));
                if (save == null)
                {
                    throw new Exception("Save file is empty or corrupted.");
                }
                return save;
            }
            else
            {
                throw new NotImplementedException();
                return InitializeCity();
            }
        }

        public World InitializeCity()
        {
            World save = new World();
            save.Citizens = new List<Core.Models.Citizen>();
            save.BankAccount = new List<Core.Models.BankAccount>();
            save.Jobs = new List<Core.Models.Job>();
            save.ShoppingCenters = new List<Core.Models.Home.ShoppingCenter>();
            save.Houses = new List<Core.Models.Home.House>();
            save.Apartments = new List<Core.Models.Home.Apartment>();
            save.Schools = new List<Core.Models.Home.School>();
            return save;
        }
    }
}
