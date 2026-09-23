using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Engine.Builders
{
    public class ShoppingCenterBuilder : IBuilder
    {
        public void Build(World world)
        {
            List<ShoppingCenter> shoppingCenters = new List<ShoppingCenter>();
            List<string> addresses = Core.Data.Address.AddressList.ToList();

            Random rnd = new Random();
            int shoppingCenterCount = rnd.Next(5, 10);
            for (int i = 0; i < shoppingCenterCount; i++)
            {
                ShoppingCenter shoppingCenter = new ShoppingCenter
                {
                    FoodPrice = rnd.Next(2, 5),
                    WaterPrice = rnd.Next(2, 5),
                    Address = addresses[rnd.Next(addresses.Count)],
                    JobCapacity = rnd.Next(2, 4),
                    Salary = rnd.Next(15, 30),
                    WorkStartTime = rnd.Next(8, 10),
                    WorkEndTime = rnd.Next(16, 18)
                };
                addresses.Remove(shoppingCenter.Address);
                world.ShoppingCenters.Add(shoppingCenter);
                world.Workplaces.Add(shoppingCenter);
            }
        }

    }
}
