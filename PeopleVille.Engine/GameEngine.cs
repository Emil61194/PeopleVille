using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Text.Json;

namespace PeopleVille.Engine
{
    public class GameEngine
    {
        private World? _world;
        private int? _currentTime;
        public bool _doPause = false;
        public event Action? Tick;

        public bool Initialize(string? filePath = null)
        {
            if (filePath == null)
            {
                _world = InitializeCity();
            }
            else
            {
                World? worldCandidate = CheckSave(filePath);

                if (worldCandidate == null) return false;
                _world = worldCandidate;
            }
            
            return true;
        }
        public void Run()
        {
            while (true)
            {
                _world.Time++;
                Tick?.Invoke();



                // Random stuff happening ( e.g heatstroke, lack of supplies in town = death, virus ) 


                // Wait 1 second
                world.currentDateTime.AddHours(1);
                Thread.Sleep(1000);
                while (_doPause)
                {
                    // Check for user input to resume or exit
                }
            }
        }

        public World? CheckSave(string filePath)
        {
            if (File.Exists(filePath))
            {
                World? save = JsonSerializer.Deserialize<World>(File.ReadAllText(filePath));
                if (save != null)
                {
                    return save;
                }
                throw new Exception("Save file is empty or corrupted.");
            }
            throw new Exception("Required file not found" + filePath);
        }

        public World InitializeCity()
        {
            World save = new World();

            ShoppingCenterBuilder shoppingCenterBuilder = new ShoppingCenterBuilder();
            shoppingCenterBuilder.BuildShoppingCenters(save);

            SchoolBuilder schoolBuilder = new SchoolBuilder();
            schoolBuilder.BuildSchools(save);

            save.Jobs = new List<Job>();
            JobsBuilder.BuildJobs(save);

            HouseBuilder houseBuilder = new HouseBuilder();
            houseBuilder.BuildHouses(save);

            ApartmentBuilder apartmentBuilder = new ApartmentBuilder();
            apartmentBuilder.BuildApartments(save);

            CitizenBuilder citizenBuilder = new CitizenBuilder();
            citizenBuilder.BuildCitizens(save,Tick);

            save.BankAccount = new List<BankAccount>();
            
            return save;
        }
    }
}
