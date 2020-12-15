using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Enemy : BaseNotifyPropertyChanged
    {
        private int _currentHitPoints;

        #region Public Properties
        public int EnemyID { get; set; }
        public string Name { get; private set; }
        public string ImageName { get; set; }
        public int MaximumHitPoints { get; private set; }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }
        public float Speed { get; private set; }
        public int EncounterRate { get; set; }
        public int BaseMissRate { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }
        #endregion
        
        public Enemy(int enemyID, string name, int maximumHitPoints, int currentHitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold, float speed, int encRate, int missRate)
        {
            EnemyID = enemyID;
            Name = name.Replace('_', ' ');
            ImageName = $"/Engine;component/Images/Monsters/{name}";
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            Speed = speed;
            EncounterRate = encRate;
            BaseMissRate = missRate;
            Inventory = new ObservableCollection<Item>();
        }
    }
}
