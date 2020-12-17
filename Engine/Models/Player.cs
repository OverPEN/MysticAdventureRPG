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
    public class Player : BaseNotifyPropertyChanged
    {
        #region Private Properties
        private String _name { get; set; }
        private String _surname { get; set; }
        private PlayerClassType _class { get; set; }
        private int _maximumHitPoints { get; set; }
        private int _currentHitPoints { get; set; }
        private int _baseDamage { get; set; }
        private float _speed { get; set; }
        private Byte _level { get; set; }
        private int _experience { get; set; }
        private int _gold { get; set; }
        private int _worldID { get; set; }
        private int _xCoordinate { get; set; }
        private int _yCoordinate { get; set; }
        #endregion

        #region Public Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
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
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            set
            {
                _maximumHitPoints = value;
                OnPropertyChanged(nameof(MaximumHitPoints));
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }
        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged(nameof(Gold));
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
        public int WorldID
        {
            get { return _worldID; }
            set
            {
                _worldID = value;
                OnPropertyChanged(nameof(WorldID));
            }
        }
        public ObservableCollection<Item> Inventory { get; set; }
        public ObservableCollection<Quest> Quests { get; set; }
        public List<Item> Weapons => Inventory.Where(i => i is Weapon).ToList();
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
            Inventory = new ObservableCollection<Item>();
            Quests = new ObservableCollection<Quest>();
        }

        public void AddItemToInventory(Item item)
        {
            if(Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) == null)
            { 
            Inventory.Add(item);
            }
            else
            {
                Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity += item.Quantity;
            }
            OnPropertyChanged(nameof(Weapons));
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) != null)
            {
                if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity <= item.Quantity)
                {
                    Inventory.Remove(Inventory.FirstOrDefault(i => i.ItemID == item.ItemID));
                }
                else
                {
                    Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity -= item.Quantity;
                }
            }

            OnPropertyChanged(nameof(Weapons));
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
