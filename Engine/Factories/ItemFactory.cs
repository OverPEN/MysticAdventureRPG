using CommonClasses.Enums;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static List<Item> _standardItems;

        static ItemFactory()
        {
            _standardItems = new List<Item>();

            _standardItems.Add(new Item(1, "Denti di serpente", 1, ItemType.Varie));
            _standardItems.Add(new Weapon(1001, "Spada Smussata", 5, 9, 9, WeaponDamageType.Taglio));
            _standardItems.Add(new Weapon(1002, "Bastone da mago scheggiato", 5, 9, 9, WeaponDamageType.Magico));
            _standardItems.Add(new Weapon(1003, "Arco da caccia storto", 5, 9, 9, WeaponDamageType.Penetrante));
            _standardItems.Add(new Weapon(1004, "Martello da fabbro usurato", 5, 9, 9, WeaponDamageType.Schianto));
        }

        public static Item CreateItem(int itemID)
        {
            Item standardItem = GetItemByID(itemID);

            if (standardItem != null)
            {
                return standardItem.Clone();
            }
            return null;
        }

        internal static Item GetItemByID(int id)
        {
            return _standardItems.FirstOrDefault(item => item.ItemID == id);
        }
    }
}
