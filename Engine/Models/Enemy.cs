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
        public int MinimumDamage { get; }
        public int MaximumDamage { get; }
        public int RewardExperiencePoints { get; }
        public int EncounterRate { get; set; }
        public int BaseMissRate { get; }
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints, int currentHitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold, float speed, int encRate, int missRate) : base(name.Replace('_', ' '),maximumHitPoints, currentHitPoints, speed, rewardGold, PlayerClassType.Enemy)
        {
            EnemyID = enemyID;
            ImageName = $"/Engine;component/Resources/EnemyImages/{name}";
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            EncounterRate = encRate;
            BaseMissRate = missRate;
        }
    }
}
