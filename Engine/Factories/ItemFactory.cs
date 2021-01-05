using CommonClasses.Enums;
using Engine.Actions;
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
        private static readonly List<Item> _standardItems = new List<Item>();

        static ItemFactory()
        {
            BuildNewItem(1, "Denti di serpente", 1, ItemType.Varie);
            BuildNewItem(2, "Pelle di serpente", 1, ItemType.Varie);
            BuildNewItem(3, "Uova di serpente", 1, ItemType.Varie);
            BuildNewItem(4, "Freccia", 1, ItemType.Munizione);

            BuildNewHealingItem(2001, "Pane", 4, 5);

            BuildNewWeapon(1001, "Spada Smussata", 5, 4, 9, WeaponDamageType.Taglio, 1.0f, 20);
            BuildNewWeapon(1002, "Bastone da mago scheggiato", 6, 6, 8, WeaponDamageType.Magico, 0.9f, 30);
            BuildNewWeapon(1003, "Arco da caccia storto", 5, 3, 10, WeaponDamageType.Penetrante, 1.2f, 10);
            BuildNewWeapon(1004, "Martello da fabbro usurato", 5, 6, 9, WeaponDamageType.Schianto, 1.8f, 25);
            BuildNewWeapon(1005, "Spada Grezza", 10, 9, 9, WeaponDamageType.Taglio, 1.0f, 20);
            BuildNewWeapon(1006, "Denti ", 0, 1, 4, WeaponDamageType.Penetrante, 1.2f, 50);
        }

        public static GroupedItem ObtainItem(int itemID, byte quantity = 1)
        {
            Item standardItem = GetItemByID(itemID);

            if (standardItem != null)
            {
                if(standardItem is Weapon)
                {
                    return new GroupedItem((standardItem as Weapon).Clone(),quantity);
                }
                return new GroupedItem(standardItem.Clone(), quantity);
            }
            return null;
        }

        private static void BuildNewItem(int itemID, string name, int price, ItemType type)
        {
            _standardItems.Add(new Item(itemID, name, price, type));
        }

        private static void BuildNewWeapon(int weaponID, string name, int price, int minDamage, int maxDamage, WeaponDamageType damageType, float weaponSpeed, int missRate)
        {
            Weapon weapon = new Weapon(weaponID, name, price, minDamage, maxDamage, damageType, weaponSpeed, missRate);
            weapon.Action = new AttackWithWeapon(weapon);
            _standardItems.Add(weapon);
        }

        private static void BuildNewHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            Item item = new Item(id, name, price, ItemType.Consumabile);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardItems.Add(item);
        }

        internal static Item GetItemByID(int id)
        {
            return _standardItems.FirstOrDefault(item => item.ItemID == id);
        }
    }
}
