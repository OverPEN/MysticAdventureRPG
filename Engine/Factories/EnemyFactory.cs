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
        public static Enemy GetEnemyByID(int enemyID)
        {
            Enemy enemy = null;
            switch (enemyID)
            {
                case 1:
                    enemy = new Enemy(1, "Serpente", 4, 4, 10, 1, 2.0f, 50, ItemFactory.ObtainItem(1006).Item as Weapon);
                    AddLootItemToEnemy(ref enemy, 1, 25, (byte)BaseRandomNumberGenerator.NumberBetween(1,3));
                    AddLootItemToEnemy(ref enemy, 2, 50, (byte)BaseRandomNumberGenerator.NumberBetween(1, 3));
                    AddLootItemToEnemy(ref enemy, 3, 25, (byte)BaseRandomNumberGenerator.NumberBetween(1, 3));
                    break;
                case 2:
                    enemy = new Enemy(2, "Bandito", 20, 20, 20, 8, 1.5f, 50, ItemFactory.ObtainItem(1001).Item as Weapon);
                    AddLootItemToEnemy(ref enemy, 1005, 25, 1);
                    break;
            }
            return enemy;
        }

        private static void AddLootItemToEnemy(ref Enemy enemy, int itemID, int percentage, byte quantity)
        {
            if (BaseRandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                enemy.AddItemToInventory(new GroupedItem(ItemFactory.GetItemByID(itemID).Clone(),quantity));
            }
        }
    }
}
