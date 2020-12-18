using CommonClasses.BaseClasses;
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
        public int EnemyID { get; set; }
        public string ImageName { get; set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; private set; }
        public int EncounterRate { get; set; }
        public int BaseMissRate { get; set; }
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints, int currentHitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold, float speed, int encRate, int missRate)
        {
            EnemyID = enemyID;
            Name = name.Replace('_', ' ');
            ImageName = $"/Engine;component/Resources/EnemyImages/{name}";
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            Gold = rewardGold;
            Speed = speed;
            EncounterRate = encRate;
            BaseMissRate = missRate;
        }
    }
}
