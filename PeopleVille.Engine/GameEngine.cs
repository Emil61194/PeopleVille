using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Text.Json;

namespace PeopleVille.Engine
{
    public class GameEngine
    {
        private World? world;
        private int? currentTime;
        public bool doPause = false;
        public event Action? Tick;

        public void Initialize(string filePath = "")
        {
            world = CheckSave(filePath);
            foreach (Citizen citizen in world.Citizens)
            {
                Tick += citizen.DoSomething;
            }
        }
        public void Run()
        {
            while (true)
            {
                // Tid, (wait e.g 1 second) CHECK
                // Citizens do something ( With Events - Delegete)
                Tick?.Invoke();



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
            save.ShoppingCenters = new List<ShoppingCenter>();

            save.Schools = new List<School>();

            save.Jobs = new List<Job>();

            CitizenBuilder citizenBuilder = new CitizenBuilder();
            save.Citizens = citizenBuilder.BuildCitizens(save);

            save.Houses = new List<House>(); // Amount of homes is dependent on the amount of last names 

            save.Apartments = new List<Apartment>(); // Amount of homes is dependent on the amount of last names 

            save.BankAccount = new List<BankAccount>();
            
            return save;
        }
    }
}
