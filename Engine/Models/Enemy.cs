using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Enemy : LivingEntity
    {
        #region Public Properties
        public int EnemyID { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }
        public int EncounterRate { get; set; }
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints, int currentHitPoints,int rewardExperiencePoints, int rewardGold, float speed, int encRate, Weapon currWeapon) : base(name.Replace('_', ' '),maximumHitPoints, currentHitPoints, speed, rewardGold, PlayerClassType.Enemy, 1, currWeapon)
        {
            EnemyID = enemyID;
            ImageName = $"/Engine;component/Resources/EnemyImages/{name}";
            RewardExperiencePoints = rewardExperiencePoints;
            EncounterRate = encRate;
        }
    }
}
