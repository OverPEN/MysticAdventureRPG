using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader susan = new Trader(1, "Susan");
            susan.AddItemToInventory(ItemFactory.ObtainItem(1001));
            susan.AddItemToInventory(ItemFactory.ObtainItem(5));
            susan.AddItemToInventory(ItemFactory.ObtainItem(6));
            susan.AddItemToInventory(ItemFactory.ObtainItem(7));

            Trader farmerTed = new Trader(2, "Farmer Ted");
            farmerTed.AddItemToInventory(ItemFactory.ObtainItem(1001));

            Trader peteTheHerbalist = new Trader(3, "Pete the Herbalist");
            peteTheHerbalist.AddItemToInventory(ItemFactory.ObtainItem(1001));

            AddTraderToList(susan);
            AddTraderToList(farmerTed);
            AddTraderToList(peteTheHerbalist);
        }

        public static Trader GetTraderByID(int id)
        {
            return _traders.FirstOrDefault(t => t.TraderID == id);
        }

        private static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.TraderID == trader.TraderID))
            {
                throw new ArgumentException($"C'è già un commerciante chiamato '{trader.Name}' con lo stesso ID");
            }

            _traders.Add(trader);
        }
    }
}
