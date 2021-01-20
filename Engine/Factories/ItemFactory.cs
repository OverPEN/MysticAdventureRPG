using CommonClasses.Enums;
using CommonClasses.ExtensionMethods;
using Engine.Actions;
using Engine.Models;
using System.Collections.Generic;
using System.Linq;
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
                LoadItemsFromNodes(data.SelectNodes("/Items/Consumables/RestoringItem"));
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
                ItemTypeEnum itemType = DetermineItemType(node.Name);

                if (itemType == ItemTypeEnum.Weapon)
                {
                    Weapon weapon = new Weapon(node.GetXmlAttributeAsInt(nameof(Weapon.ItemID)), node.GetXmlAttributeAsString(nameof(Weapon.Name)), node.GetXmlAttributeAsInt(nameof(Weapon.Price)), node.GetXmlAttributeAsInt(nameof(Weapon.MinimumDamage)), node.GetXmlAttributeAsInt(nameof(Weapon.MaximumDamage)), node.GetXmlAttributeAsDamageType(nameof(Weapon.DamageType)), node.GetXmlAttributeAsFloat(nameof(Weapon.WeaponSpeed)), node.GetXmlAttributeAsInt(nameof(Weapon.MissRate)));
                    weapon.Action = new Attack(weapon);
                    weapon.UsableBy = new List<PlayerClassTypeEnum>();
                    foreach(byte element in node.GetXmlAttributeAsByteList(nameof(Weapon.UsableBy)))
                    {
                        weapon.UsableBy.Add((PlayerClassTypeEnum)element);
                    }
                    _standardItems.Add(weapon);
                }
                else if (itemType == ItemTypeEnum.Consumable)
                {
                    RestoringItem restoringItem = new RestoringItem(node.GetXmlAttributeAsInt(nameof(Item.ItemID)), node.GetXmlAttributeAsString(nameof(RestoringItem.Name)), node.GetXmlAttributeAsInt(nameof(RestoringItem.Price)), node.GetXmlAttributeAsInt(nameof(RestoringItem.PointsToRestore)), node.GetXmlAttributeAsString(nameof(RestoringItem.Target)));
                    restoringItem.Action = new Restore(restoringItem);
                    restoringItem.UsableBy = new List<PlayerClassTypeEnum>();

                    foreach (byte element in node.GetXmlAttributeAsByteList(nameof(RestoringItem.UsableBy)))
                    {
                        restoringItem.UsableBy.Add((PlayerClassTypeEnum)element);
                    }
                    _standardItems.Add(restoringItem);
                }
                else if(itemType == ItemTypeEnum.Armor)
                {

                }
                else if (itemType == ItemTypeEnum.Miscellaneous)
                {
                    Item item = new Item(node.GetXmlAttributeAsInt(nameof(Item.ItemID)), node.GetXmlAttributeAsString(nameof(Item.Name)), node.GetXmlAttributeAsInt(nameof(Item.Price)), itemType);

                    _standardItems.Add(item);
                }
            }
        }

        private static ItemTypeEnum DetermineItemType(string itemType)
        {
            switch (itemType)
            {
                case nameof(Weapon):
                    return ItemTypeEnum.Weapon;
                case nameof(RestoringItem):
                    return ItemTypeEnum.Consumable;
                default:
                    return ItemTypeEnum.Miscellaneous;
            }
        }
        #endregion
    }
}
