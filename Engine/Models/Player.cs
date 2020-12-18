using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Factories;
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
        private Byte _level { get; set; }
        private int _experience { get; set; }
        private int _baseDamage { get; set; }
        private int _worldID { get; set; }
        private int _xCoordinate { get; set; }
        private int _yCoordinate { get; set; }
        #endregion

        #region Public Properties
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
        public int WorldID
        {
            get { return _worldID; }
            set
            {
                _worldID = value;
                OnPropertyChanged(nameof(WorldID));
            }
        }
        public int XCoordinate
        {
            get { return _xCoordinate; }
            set
            {
                _xCoordinate = value;
                OnPropertyChanged(nameof(XCoordinate));
            }
        }
        public int YCoordinate
        {
            get { return _yCoordinate; }
            set
            {
                _yCoordinate = value;
                OnPropertyChanged(nameof(YCoordinate));
            }
        }
        public WeaponDamageType BaseDamageType { get; set; } 
        public ObservableCollection<Quest> Quests { get; set; }

        #endregion

        public Player(string name, int maxHitPoints, int currHitPoints, float speed, int gold, int worldID, int xCoord, int yCoord, PlayerClassType chosenClass, byte level, int experience) : base(name, maxHitPoints, currHitPoints, speed, gold, chosenClass )
        {
            ClassBaseValues defaultValues = new ClassBaseValues(chosenClass);

            Level = level;
            Experience = experience;
            BaseDamage = defaultValues.BaseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            WorldID = worldID;
            XCoordinate = xCoord;
            YCoordinate = yCoord;
            Quests = new ObservableCollection<Quest>();
      
        }

        public Player(string name, PlayerClassType chosenClass) : base(name, new ClassBaseValues(chosenClass).HitPoints, new ClassBaseValues(chosenClass).HitPoints, new ClassBaseValues(chosenClass).Speed, new ClassBaseValues(chosenClass).Gold, chosenClass)
        {
            ClassBaseValues defaultValues = new ClassBaseValues(chosenClass);
            Level = 1;
            Experience = 0;
            BaseDamage = defaultValues.BaseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            WorldID = 0;
            XCoordinate = 0;
            YCoordinate = 0;
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
