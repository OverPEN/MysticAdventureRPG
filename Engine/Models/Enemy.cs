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

        public string Name { get; private set; }
        public string ImageName { get; set; }
        public int MaximumHitPoints { get; private set; }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }
        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }
        public float Speed { get; private set; }
        public int EncounterRate { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }

        public Enemy(string name, string imageName, int maximumHitPoints, int currentHitPoints, int rewardExperiencePoints, int rewardGold, float speed, int encRate)
        {
            Name = name;
            ImageName = string.Format("/Engine;component/Images/Monsters/{0}", name);
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            Speed = speed;
            EncounterRate = encRate;
            Inventory = new ObservableCollection<Item>();
        }
    }
}
