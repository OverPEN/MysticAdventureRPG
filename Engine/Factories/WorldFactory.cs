using Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses.ExtensionMethods;
using System.Xml;

namespace Engine.Factories
{
    public static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";

        public static World CreateWorld()
        {
            World world = new World();

            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Locations").GetXmlAttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world, rootImagePath, data.SelectNodes("/Locations/Location"));

            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }

            return world;
        }

        #region Functions
        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Location location = new Location(node.GetXmlAttributeAsInt(nameof(Location.LocationID)), node.GetXmlAttributeAsInt(nameof(Location.XCoordinate)), node.GetXmlAttributeAsInt(nameof(Location.YCoordinate)), node.GetXmlAttributeAsString(nameof(Location.Name)), node.SelectSingleNode("./Description")?.InnerText ?? "", $".{rootImagePath}{node.GetXmlAttributeAsString(nameof(Location.ImageName))}");

                AddEnemies(location, node.SelectNodes("./Enemies/Enemy"));
                AddQuests(location, node.SelectNodes("./Quests/Quest"));
                AddTrader(location, node.SelectSingleNode("./Trader"));

                world.AddLocation(location);
            }
        }

        private static void AddEnemies(Location location, XmlNodeList enemies)
        {
            if (enemies == null)
            {
                return;
            }

            foreach (XmlNode monsterNode in enemies)
            {
                location.AddEnemyToLocation(monsterNode.GetXmlAttributeAsInt(nameof(EnemyEncounter.EnemyID)), monsterNode.GetXmlAttributeAsInt(nameof(EnemyEncounter.EncounterRate)));

            }
        }

        private static void AddQuests(Location location, XmlNodeList quests)
        {
            if (quests == null)
            {
                return;
            }

            foreach (XmlNode questNode in quests)
            {
                location.AddQuestToLocation(questNode.GetXmlAttributeAsInt(nameof(Quest.QuestID)));

            }
        }

        private static void AddTrader(Location location, XmlNode traderHere)
        {
            if (traderHere == null)
            {
                return;
            }

            location.AddTraderToLocation(traderHere.GetXmlAttributeAsInt(nameof(Trader.TraderID)));

        }
        #endregion
    }
}
