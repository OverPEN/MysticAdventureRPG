using CommonClasses.BaseClasses;
using CommonClasses.ExtensionMethods;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Factories
{
    public static class EnemyFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Enemies.xml";

        private static readonly List<Enemy> _baseEnemies = new List<Enemy>();

        static EnemyFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Enemies").GetXmlAttributeAsString("RootImagePath");

                LoadEnemiesFromNodes(data.SelectNodes("/Enemies/Enemy"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        #region Functions
        private static void LoadEnemiesFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Enemy enemy = new Enemy(node.GetXmlAttributeAsInt(nameof(Enemy.EnemyID)), node.GetXmlAttributeAsString(nameof(Enemy.Name)), node.GetXmlAttributeAsInt(nameof(Enemy.MaximumHitPoints)), node.GetXmlAttributeAsInt(nameof(Enemy.RewardExperiencePoints)), node.GetXmlAttributeAsInt(nameof(Enemy.Gold)), node.GetXmlAttributeAsFloat(nameof(Enemy.Speed)), ItemFactory.ObtainItem(node.GetXmlAttributeAsInt(nameof(Enemy.CurrentWeapon))).Item as Weapon, $".{rootImagePath}{node.GetXmlAttributeAsString(nameof(Enemy.ImageName))}");

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        enemy.AddItemToLootTable(lootItemNode.GetXmlAttributeAsInt(nameof(LootItem.ItemID)), lootItemNode.GetXmlAttributeAsInt(nameof(LootItem.DropRate)), lootItemNode.GetXmlAttributeAsInt("MinQuantity"), lootItemNode.GetXmlAttributeAsInt("MaxQuantity"));
                    }
                }

                _baseEnemies.Add(enemy);
            }
        }

        public static Enemy GetEnemyByID(int enemyID)
        {
            return _baseEnemies.FirstOrDefault(m => m.EnemyID == enemyID)?.GetNewInstance();
        }
        #endregion

    }
}
