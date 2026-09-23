using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Collections.Concurrent;
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
        public event Action? Tick;

        public ConcurrentBag<object> actionsEachTick = new ConcurrentBag<object>();
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

            var builders = new List<IBuilder>
            {
                new ShoppingCenterBuilder(),
                new SchoolBuilder(),
                new JobsBuilder(),
                new HouseBuilder(),
                new ApartmentBuilder(),
                new CitizenBuilder(this)
            };

            string appDir = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
                "PeopleVille.Handicaps", "bin", "Debug", "net10.0", "PeopleVille.Handicaps.dll"));

            builders.AddRange(CheckExternalBuilders(appDir));

            foreach (var builder in builders)
            {
                builder.Build(save);
            }
            
            return save;
        }

        private IEnumerable<IBuilder> CheckExternalBuilders(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            var assembly = System.Reflection.Assembly.LoadFrom(filePath);
            var builderTypes = assembly.GetTypes()
                .Where(t => typeof(IBuilder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in builderTypes)
            {
                object? instance = null;
                try
                {
                    instance = Activator.CreateInstance(type, this);
                }
                catch (MissingMethodException)
                {
                    instance = Activator.CreateInstance(type);
                }

                if (instance is IBuilder builder)
                {
                    yield return builder;
                }
            }
        }

        public void CheckWorld()
        {
            if (_world == null) throw new Exception("World is not initialized.");
        }

        public House GetHouseByAddress(string address)
        {
            CheckWorld();
            House? house = _world.Houses.FirstOrDefault(h => h.Address == address);
            if (house == null) throw new Exception("House not found.");
            return house;
        }
        public Apartment GetApartmentByAddress(string address)
        {
            CheckWorld();
            Apartment? apartment = _world.Apartments.FirstOrDefault(a => a.Address == address);
            if (apartment == null) throw new Exception("Apartment not found.");
            return apartment;
        }
        public Citizen GetCitizenById(int id)
        {
            CheckWorld();
            Citizen? citizen = _world.Citizens.FirstOrDefault(c => c.Id == id);
            if (citizen == null) throw new Exception("Citizen not found.");
            return citizen;
        }
        public List<object> GetAllHomes()
        {
            CheckWorld();
            List<object> homes = new List<object>();
            homes.AddRange(_world.Houses);
            homes.AddRange(_world.Apartments);
            return homes;
        }
        public List<Citizen> GetAllCitizens()
        {
            CheckWorld();
            return _world.Citizens;
        }

        public List<ShoppingCenter> GetAllWorkplaces()
        {
            CheckWorld();
            return _world.ShoppingCenters;
        }

        public IWorkplace GetWorkplaceByAddress(string address)
        {
            CheckWorld();
            IWorkplace? workplace = _world.Workplaces.FirstOrDefault(w => w.Address == address);
            if (workplace == null) throw new Exception("Workplace not found.");
            return workplace;
        }
    }
}
