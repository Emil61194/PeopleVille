using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine.Builders;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Reflection;
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
                try
                {
                    Tick?.Invoke();

                    // Random stuff happening ( e.g heatstroke, lack of supplies in town = death, virus )

                    // Advance 1 hour
                    if (_world != null)
                    {
                        _world.currentDateTime = _world.currentDateTime.AddHours(1);
                    }

                    // Wait 1 second
                    Thread.Sleep(1000);
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
                catch (Exception exception)
                {
                    Console.WriteLine($"Tick failed: {exception}");
                    Thread.Sleep(1000);
                }
            }
        }

        public static World CheckSave(string filePath)
        {
            if (File.Exists(filePath))
            {
                World save = JsonSerializer.Deserialize<World>(File.ReadAllText(filePath))
                    ?? throw new Exception("Save file is empty or corrupted.");
                return save;
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
            string dllPath = string.Empty;

            if (CheckIfRunningRelease())
            {
                dllPath = Path.GetFullPath(
                    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
                    "PeopleVille.Handicaps", "bin", "Release", "net10.0", "PeopleVille.Handicaps.dll"));
            }
            else
            {
                dllPath = Path.GetFullPath(
                    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
                    "PeopleVille.Handicaps", "bin", "Debug", "net10.0", "PeopleVille.Handicaps.dll"));
            }
            
            if (File.Exists(dllPath))
            {
                builders.AddRange(CheckExternalBuilders(dllPath));
            }

            foreach (IBuilder builder in builders)
            {
                builder.Build(save);
            }

            return save;
        }

        private bool CheckIfRunningRelease()
        {
            #if DEBUG
                return false;
            #else
                return true;
            #endif
        }

        private IEnumerable<IBuilder> CheckExternalBuilders(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            Assembly assembly = Assembly.LoadFrom(filePath);
            IEnumerable<Type> builderTypes = assembly.GetTypes()
                .Where(t => typeof(IBuilder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (Type type in builderTypes)
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

        [MemberNotNull(nameof(_world))]
        public World CheckWorld()
        {
            if (_world == null) throw new Exception("World is not initialized.");
            return _world;
        }

        public House GetHouseByAddress(string address)
        {
            World world = CheckWorld();
            House house = world.Houses.FirstOrDefault(h => h.Address == address)
                ?? throw new Exception("House not found.");
            return house;
        }
        public Apartment GetApartmentByAddress(string address)
        {
            World world = CheckWorld();
            Apartment apartment = world.Apartments.FirstOrDefault(a => a.Address == address)
                ?? throw new Exception("Apartment not found.");
            return apartment;
        }
        public Citizen GetCitizenById(int id)
        {
            World world = CheckWorld();
            Citizen citizen = world.Citizens.FirstOrDefault(c => c.Id == id)
                ?? throw new Exception("Citizen not found.");
            return citizen;
        }
        public List<object> GetAllHomes()
        {
            World world = CheckWorld();
            List<object> homes = [.. world.Houses.Cast<object>(), .. world.Apartments.Cast<object>()];
            return homes;
        }
        public List<Citizen> GetAllCitizens()
        {
            World world = CheckWorld();
            return world.Citizens;
        }

        public List<ShoppingCenter> GetAllWorkplaces()
        {
            World world = CheckWorld();
            return world.ShoppingCenters;
        }

        public IWorkplace GetWorkplaceByAddress(string address)
        {
            World world = CheckWorld();
            IWorkplace? workplace = world.Workplaces.FirstOrDefault(w => w.Address == address);
            if (workplace == null) throw new Exception("Workplace not found.");
            return workplace;
        }
    }
}