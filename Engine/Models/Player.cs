using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        #region Private Properties
        private PlayerClassType _class { get; set; }
        private Byte _level { get; set; }
        private int _experience { get; set; }
        private int _baseDamage { get; set; }      
        #endregion

        #region Public Properties
        public PlayerClassType Class
        {
            get { return _class; }
            set
            {
                _class = value;
                OnPropertyChanged(nameof(Class));
            }
        }
        public byte Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public int Experience
        {
            get { return _experience; }
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
            }
        }
        public int BaseDamage
        {
            get { return _baseDamage; }
            set
            {
                _baseDamage = value;
                OnPropertyChanged(nameof(BaseDamage));
            }
        }
        public WeaponDamageType BaseDamageType { get; set; } 
        public ObservableCollection<Quest> Quests { get; set; }

        #endregion

        public Player(string chosenName, string chosenSurname, PlayerClassType chosenClass)
        {
            PlayerClassBaseValues defaultValues = new PlayerClassBaseValues(chosenClass);

            Name = chosenName;
            Surname = chosenSurname;
            Class = chosenClass;
            Level = 1;
            Experience = 0;
            BaseDamage = defaultValues.BaseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            Speed = defaultValues.Speed;
            MaximumHitPoints = defaultValues.HitPoints;
            CurrentHitPoints = defaultValues.HitPoints;
            Gold = defaultValues.Gold;
            XCoordinate = 0;
            YCoordinate = 0;
            WorldID = 0;
            Quests = new ObservableCollection<Quest>();
        }

        public bool HasAllTheseItems(List<Item> items)
        {
            foreach (Item item in items)
            {
                if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) != null)
                {
                    if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity < item.Quantity)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }             
            }
            return true;
        }
    }
}
