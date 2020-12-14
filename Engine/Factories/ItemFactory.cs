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
            _standardItems.Add(new Item(2, "Pelle di serpente", 1, ItemType.Varie));
            _standardItems.Add(new Item(3, "Uova di serpente", 1, ItemType.Varie));
            _standardItems.Add(new Item(4, "Freccia", 1, ItemType.Munizione));
            _standardItems.Add(new Weapon(1001, "Spada Smussata", 5, 9, 9, WeaponDamageType.Taglio, 1.0f));
            _standardItems.Add(new Weapon(1002, "Bastone da mago scheggiato", 5, 8, 8, WeaponDamageType.Magico, 0.9f));
            _standardItems.Add(new Weapon(1003, "Arco da caccia storto", 5, 9, 9, WeaponDamageType.Penetrante, 1.2f));
            _standardItems.Add(new Weapon(1004, "Martello da fabbro usurato", 5, 9, 9, WeaponDamageType.Schianto, 1.8f));
        }

        public static Item CreateItem(int itemID, byte quantity = 1)
        {
            Item standardItem = GetItemByID(itemID);

            if (standardItem != null)
            {
                return standardItem.Clone(quantity);
            }
            return null;
        }

        internal static Item GetItemByID(int id)
        {
            return _standardItems.FirstOrDefault(item => item.ItemID == id);
        }
    }
}
