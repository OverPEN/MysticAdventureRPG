using CommonClasses.Enums;
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
                    Weapon weapon = new Weapon(GetXmlAttributeAsInt(node, nameof(Weapon.ItemID)), GetXmlAttributeAsString(node, nameof(Weapon.Name)), GetXmlAttributeAsInt(node, nameof(Weapon.Price)), GetXmlAttributeAsInt(node, nameof(Weapon.MinimumDamage)), GetXmlAttributeAsInt(node, nameof(Weapon.MaximumDamage)), GetXmlAttributeAsDamageType(node, nameof(Weapon.DamageType)), GetXmlAttributeAsFloat(node, nameof(Weapon.WeaponSpeed)), GetXmlAttributeAsInt(node, nameof(Weapon.MissRate)));
                    weapon.Action = new AttackWithWeapon(weapon);

                    _standardItems.Add(weapon);
                }
                else if (itemType == ItemType.Consumable)
                {
                    HealingItem healingItem = new HealingItem(GetXmlAttributeAsInt(node, nameof(Item.ItemID)), GetXmlAttributeAsString(node, nameof(HealingItem.Name)), GetXmlAttributeAsInt(node, nameof(HealingItem.Price)), GetXmlAttributeAsInt(node, nameof(HealingItem.HitPointsToHeal)));

                    healingItem.Action = new Heal(healingItem);

                    _standardItems.Add(healingItem);
                }
                else if(itemType == ItemType.Armor)
                {

                }
                else if (itemType == ItemType.Munition)
                {
                    Item item = new Item(GetXmlAttributeAsInt(node, nameof(Item.ItemID)), GetXmlAttributeAsString(node, nameof(Item.Name)), GetXmlAttributeAsInt(node, nameof(Item.Price)), itemType);

                    _standardItems.Add(item);
                }
                else if (itemType == ItemType.Miscellaneous)
                {
                    Item item = new Item(GetXmlAttributeAsInt(node, nameof(Item.ItemID)), GetXmlAttributeAsString(node, nameof(Item.Name)), GetXmlAttributeAsInt(node, nameof(Item.Price)), itemType);

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

        private static int GetXmlAttributeAsInt(XmlNode node, string attributeName)
        {
            return Convert.ToInt32(GetXmlAttribute(node, attributeName));
        }

        private static float GetXmlAttributeAsFloat(XmlNode node, string attributeName)
        {
            return Convert.ToSingle(GetXmlAttribute(node, attributeName));
        }

        private static string GetXmlAttributeAsString(XmlNode node, string attributeName)
        {
            return GetXmlAttribute(node, attributeName);
        }

        private static WeaponDamageType GetXmlAttributeAsDamageType(XmlNode node, string attributeName)
        {
            string attribute = GetXmlAttribute(node, attributeName);

            switch (attribute)
            {
                case "Taglio":
                    return WeaponDamageType.Taglio;
                case "Magico":
                    return WeaponDamageType.Magico;
                case "Schianto":
                    return WeaponDamageType.Schianto;
                default:
                    return WeaponDamageType.Penetrante;
            }

        }

        private static string GetXmlAttribute(XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute == null)
            {
                throw new ArgumentException($"L'attributo '{attributeName}' non esiste");
            }

            return attribute.Value;
        }
        #endregion
    }
}
