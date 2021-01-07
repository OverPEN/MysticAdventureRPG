using CommonClasses.Enums;
using CommonClasses.ExtensionMethods;
using Engine.Actions;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        private static readonly List<Item> _standardItems = new List<Item>();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/Items/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/Items/Munitions/Munition"));
                LoadItemsFromNodes(data.SelectNodes("/Items/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/Items/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        #region Functions
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

        internal static Item GetItemByID(int id)
        {
            return _standardItems.FirstOrDefault(item => item.ItemID == id);
        }

        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                ItemType itemType = DetermineItemType(node.Name);

                if (itemType == ItemType.Weapon)
                {
                    Weapon weapon = new Weapon(node.GetXmlAttributeAsInt(nameof(Weapon.ItemID)), node.GetXmlAttributeAsString(nameof(Weapon.Name)), node.GetXmlAttributeAsInt(nameof(Weapon.Price)), node.GetXmlAttributeAsInt(nameof(Weapon.MinimumDamage)), node.GetXmlAttributeAsInt(nameof(Weapon.MaximumDamage)), node.GetXmlAttributeAsDamageType(nameof(Weapon.DamageType)), node.GetXmlAttributeAsFloat(nameof(Weapon.WeaponSpeed)), node.GetXmlAttributeAsInt(nameof(Weapon.MissRate)));
                    weapon.Action = new AttackWithWeapon(weapon);

                    _standardItems.Add(weapon);
                }
                else if (itemType == ItemType.Consumable)
                {
                    HealingItem healingItem = new HealingItem(node.GetXmlAttributeAsInt(nameof(Item.ItemID)), node.GetXmlAttributeAsString(nameof(HealingItem.Name)), node.GetXmlAttributeAsInt(nameof(HealingItem.Price)), node.GetXmlAttributeAsInt(nameof(HealingItem.HitPointsToHeal)));

                    healingItem.Action = new Heal(healingItem);

                    _standardItems.Add(healingItem);
                }
                else if(itemType == ItemType.Armor)
                {

                }
                else if (itemType == ItemType.Munition)
                {
                    Item item = new Item(node.GetXmlAttributeAsInt(nameof(Item.ItemID)), node.GetXmlAttributeAsString(nameof(Item.Name)), node.GetXmlAttributeAsInt(nameof(Item.Price)), itemType);

                    _standardItems.Add(item);
                }
                else if (itemType == ItemType.Miscellaneous)
                {
                    Item item = new Item(node.GetXmlAttributeAsInt(nameof(Item.ItemID)), node.GetXmlAttributeAsString(nameof(Item.Name)), node.GetXmlAttributeAsInt(nameof(Item.Price)), itemType);

                    _standardItems.Add(item);
                }
            }
        }

        private static ItemType DetermineItemType(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return ItemType.Weapon;
                case "HealingItem":
                    return ItemType.Consumable;
                case "Munition":
                    return ItemType.Munition;
                default:
                    return ItemType.Miscellaneous;
            }
        }
        #endregion
    }
}
