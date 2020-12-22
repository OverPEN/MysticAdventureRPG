using CommonClasses.BaseClasses;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class EnemyFactory
    {
        private static readonly List<Enemy> _standardEnemies = new List<Enemy>();

        static EnemyFactory()
        {
            BuildNewEnemy(1, "Serpente", 4, 4, 10, 1, 2.0f, 50, ItemFactory.ObtainItem(1006) as Weapon);
            AddLootItemToEnemy(1, 1, 25);
            AddLootItemToEnemy(1, 2, 50);
            AddLootItemToEnemy(1, 3, 25);
            BuildNewEnemy(2, "Bandito", 20, 20, 20, 8, 1.5f, 50, ItemFactory.ObtainItem(1001) as Weapon);
            AddLootItemToEnemy(2, 1005, 25);

        }

        private static void BuildNewEnemy(int enemyID, string name, int maxHitPoints, int currHitPoints, int rewardExperiencePoints, int rewardGold, float speed, int encRate, Weapon weapon)
        {
            _standardEnemies.Add(new Enemy(enemyID, name, maxHitPoints, currHitPoints, rewardExperiencePoints, rewardGold, speed, encRate, weapon));
        }

        public static Enemy GetEnemyByID(int enemyID)
        {
            return _standardEnemies.FirstOrDefault(enemy => enemy.EnemyID == enemyID);
        }

        private static void AddLootItemToEnemy(int enemyID, int itemID, int percentage)
        {
            if (BaseRandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                GetEnemyByID(enemyID).Inventory.Add(ItemFactory.GetItemByID(itemID).Clone());
            }
        }
    }
}
