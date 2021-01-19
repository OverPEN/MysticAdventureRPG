using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Factories;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Enemy : LivingEntity
    {
        private readonly List<LootItem> _lootTable = new List<LootItem>();


        #region Public Properties
        public int EnemyID { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints,int rewardExperiencePoints, int rewardGold, float speed, Weapon currWeapon, string imageName) : base(name.Replace('_', ' '),maximumHitPoints, maximumHitPoints, speed, rewardGold, PlayerClassTypeEnum.Enemy, 1, currWeapon)
        {
            EnemyID = enemyID;
            ImageName = imageName;
            RewardExperiencePoints = rewardExperiencePoints;
        }

        public void AddItemToLootTable(int itemID, int dropRate, int minQuantity, int maxQuantity)
        {
            //Se l'oggetto è già presente nella loot table lo rimuovo
            _lootTable.RemoveAll(ip => ip.ItemID == itemID);

            _lootTable.Add(new LootItem(itemID, dropRate, minQuantity, maxQuantity));
        }

        public Enemy GetNewInstance()
        {
            // Genero una nuova istanza di enemy
            Enemy enemy =
                new Enemy(EnemyID, Name, MaximumHitPoints, RewardExperiencePoints, Gold, Speed, CurrentWeapon, ImageName);

            foreach (LootItem itemLoot in _lootTable)
            {
                // Riassegno per sicurezza la loot table
                enemy.AddItemToLootTable(itemLoot.ItemID, itemLoot.DropRate, itemLoot.Quantity, itemLoot.Quantity);

                // Popolo l'inventario con ciò che contiene la loot table
                if (BaseRandomNumberGenerator.NumberBetween(1, 100) <= itemLoot.DropRate)
                {
                    enemy.AddItemToInventory(ItemFactory.ObtainItem(itemLoot.ItemID, itemLoot.Quantity));
                }
            }

            return enemy;
        }
    }
}
