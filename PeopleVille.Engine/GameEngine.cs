using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace PeopleVille.Engine
{
    public class GameEngine(EventPublisher eventPublisher)
    {
        private bool GameRunning = false;
        private World? _world;
        private EventPublisher _eventPublisher = eventPublisher;
        public World? CurrentWorld => _world;
        public bool Ready = false;
        public bool _doPause = false;
        public event Action Tick = delegate { };
        public ConcurrentBag<object> ActionsEachTickChanged { get; } = new();

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

                _world = worldCandidate;
                if (worldCandidate == null) return false;
            }

            Ready = true;
            return Ready;
        }
        public async Task Run()
        {
            if (GameRunning)
            {
                return;
            }

            GameRunning = true;
            Console.WriteLine("Running Game");
            while (GameRunning)
            {
                Tick?.Invoke();
                await _eventPublisher.PublishEvent("Yo");



                // Random stuff happening ( e.g heatstroke, lack of supplies in town = death, virus )


                // Wait 1 second
                if (_world != null)
                {
                    _world.currentDateTime = _world.currentDateTime.AddHours(1);
                }

                await Task.Delay(1000);
                while (_doPause)
                {
                    await Task.Delay(50);
                    // Check for user input to resume or exit
                }

                foreach (var action in actionsEachTick)
                {
                    await _eventPublisher.PublishEvent(action);
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
            World save = new();

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
        [MemberNotNull(nameof(_world))]
        public void CheckWorld()
        {
            if (_world == null) throw new Exception("World is not initialized.");
            return _world;
        }

        public House GetHouseByAddress(string address)
        {
            World world = CheckWorld();
            House? house = world.Houses.FirstOrDefault(h => h.Address == address);
            if (house == null) throw new Exception("House not found.");
            return house;
        }
        public Apartment GetApartmentByAddress(string address)
        {
            World world = CheckWorld();
            Apartment? apartment = world.Apartments.FirstOrDefault(a => a.Address == address);
            if (apartment == null) throw new Exception("Apartment not found.");
            return apartment;
        }
        public Citizen GetCitizenById(int id)
        {
            World world = CheckWorld();
            Citizen? citizen = world.Citizens.FirstOrDefault(c => c.Id == id);
            if (citizen == null) throw new Exception("Citizen not found.");
            return citizen;
        }
        public List<IPrivateHome> GetAllHomes()
        {
            World world = CheckWorld();
            List<IPrivateHome> homes = new List<IPrivateHome>();
            homes.AddRange(world.Houses);
            homes.AddRange(world.Apartments);
            return homes;
        }
        public List<Citizen> GetAllCitizens()
        {
            World world = CheckWorld();
            return world.Citizens;
        }

        public List<IWorkplace> GetAllWorkplaces()
        {
            World world = CheckWorld();
            List<IWorkplace> workplaces = new List<IWorkplace>();
            workplaces.AddRange(world.ShoppingCenters);
            return workplaces;
        }
    }
}
