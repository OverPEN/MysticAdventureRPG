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
            BuildNewEnemy(1, "Serpente", 4, 4, 1, 4, 1000, 1, 2.0f, 100, 50);

            AddLootItemToEnemy(1, 1, 25);
            AddLootItemToEnemy(1, 2, 50);
            AddLootItemToEnemy(1, 3, 25);
        }






        private static void BuildNewEnemy(int enemyID, string name, int maxHitPoints, int currHitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold, float speed, int encRate, int missRate)
        {
            _standardEnemies.Add(new Enemy(enemyID, name, maxHitPoints, currHitPoints, minDamage, maxDamage, rewardExperiencePoints, rewardGold, speed, encRate, missRate));
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
