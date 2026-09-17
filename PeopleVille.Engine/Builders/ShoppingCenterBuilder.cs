using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class ShoppingCenterBuilder
    {
        public void BuildShoppingCenters(World world)
        {
            List<ShoppingCenter> shoppingCenters = new List<ShoppingCenter>();
            List<string> addresses = Core.Data.Address.AddressList.ToList();

            Random rnd = new Random();
            int shoppingCenterCount = rnd.Next(1, 3);
            for (int i = 0; i < shoppingCenterCount; i++)
            {
                ShoppingCenter shoppingCenter = new ShoppingCenter
                {
                    Address = addresses[rnd.Next(addresses.Count)]
                };
                addresses.Remove(shoppingCenter.Address);
                world.ShoppingCenters.Add(shoppingCenter);
                world.Workplaces.Add(shoppingCenter);
            }
        }

    }
}
