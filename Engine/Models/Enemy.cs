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
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints,int rewardExperiencePoints, int rewardGold, float speed, Weapon currWeapon) : base(name.Replace('_', ' '),maximumHitPoints, maximumHitPoints, speed, rewardGold, PlayerClassType.Enemy, 1, currWeapon)
        {
            EnemyID = enemyID;
            ImageName = $"/Engine;component/Resources/EnemyImages/{name}";
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}
