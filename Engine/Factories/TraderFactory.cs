using CommonClasses.ExtensionMethods;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";
        private static List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        #region Functions

        private static void LoadTradersFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Trader trader = new Trader(node.GetXmlAttributeAsInt(nameof(Trader.TraderID)), node.SelectSingleNode($"./{nameof(Trader.Name)}")?.InnerText ?? "");

                foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
                {
                    trader.AddItemToInventory(ItemFactory.ObtainItem(childNode.GetXmlAttributeAsInt(nameof(Item.ItemID)), childNode.GetXmlAttributeAsByte("Quantity")));
                }

                AddTraderToList(trader);
            }
        }

        public static void ReloadTraders()
        {
            _traders = new List<Trader>();
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
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
        #endregion
    }
}
