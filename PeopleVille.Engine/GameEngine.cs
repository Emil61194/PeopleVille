using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Collections.Concurrent;
using System.Text.Json;

namespace PeopleVille.Engine
{
    public class GameEngine(EventPublisher eventPublisher)
    {
        private World? _world;
        private EventPublisher _eventPublisher = eventPublisher;
        public World? CurrentWorld => _world;
        public bool Ready = false;
        public bool _doPause = false;
        public event Action? Tick;
        public required ConcurrentBag<object> ActionsEachTickChanged;

        public ConcurrentBag<object> actionsEachTick = [];
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
                Ready = true;
            }

            return true;
        }
        public void Run()
        {
            while (true)
            {
                Tick?.Invoke();



                // Random stuff happening ( e.g heatstroke, lack of supplies in town = death, virus ) 


                // Wait 1 second
                if (_world != null)
                {
                    _world.currentDateTime = _world.currentDateTime.AddHours(1);
                }

                Thread.Sleep(1000);
                while (_doPause)
                {
                    // Check for user input to resume or exit
                }

                foreach (var action in actionsEachTick)
                {
                    _eventPublisher.PublishEvent(action);
                }
                actionsEachTick.Clear();
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
            citizenBuilder.BuildCitizens(save, Tick ?? (() => { }), ActionsEachTickChanged);

            save.BankAccount = new List<BankAccount>();

            return save;
        }
        public House GetHouseByAddress(string address)
        {
            if (_world == null) throw new Exception("World is not initialized.");
            House? house = _world.Houses.FirstOrDefault(h => h.Address == address);
            if (house == null) throw new Exception("House not found.");
            return house;
        }
        public Apartment GetApartmentByAddress(string address)
        {
            if (_world == null) throw new Exception("World is not initialized.");
            Apartment? apartment = _world.Apartments.FirstOrDefault(a => a.Address == address);
            if (apartment == null) throw new Exception("Apartment not found.");
            return apartment;
        }
        public Citizen GetCitizenById(int id)
        {
            if (_world == null) throw new Exception("World is not initialized.");
            Citizen? citizen = _world.Citizens.FirstOrDefault(c => c.Id == id);
            if (citizen == null) throw new Exception("Citizen not found.");
            return citizen;
        }
    }
}