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
        public static Enemy GetEnemy(int enemyID)
        {
            switch (enemyID)
            {
                case 1:
                    Enemy snake =
                        new Enemy("Snake", "Snake.png", 4, 4, 5, 1, 2.0f,40);

                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;

                //case 2:
                //    Enemy rat =
                //        new Enemy("Rat", "Rat.png", 5, 5, 5, 1);

                //    AddLootItem(rat, 9003, 25);
                //    AddLootItem(rat, 9004, 75);

                //    return rat;

                //case 3:
                //    Enemy giantSpider =
                //        new Enemy("Giant Spider", "GiantSpider.png", 10, 10, 10, 3);

                //    AddLootItem(giantSpider, 9005, 25);
                //    AddLootItem(giantSpider, 9006, 75);

                //    return giantSpider;

                default:
                    throw new ArgumentException(string.Format("EnemyType '{0}' does not exist", enemyID));
            }
        }

        private static void AddLootItem(Enemy enemy, int itemID, int percentage)
        {
            if (BaseRandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                enemy.Inventory.Add(ItemFactory.GetItemByID(itemID).Clone());
            }
        }
    }
}
