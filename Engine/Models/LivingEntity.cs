using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotifyPropertyChanged
    {
        #region Private Properties
        private String _name { get; set; }
        private String _surname { get; set; }
        private int _maximumHitPoints { get; set; }
        private int _currentHitPoints { get; set; }
        private float _speed { get; set; }
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
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
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
        public ObservableCollection<Item> Inventory { get; set; }
        public List<Item> Weapons => Inventory.Where(i => i is Weapon).ToList();
        #endregion

        protected LivingEntity()
        {
            Inventory = new ObservableCollection<Item>();
        }

        #region Functions
        public void AddItemToInventory(Item item)
        {
            if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) == null || item.IsUnique)
            {
                if (item.IsUnique)
                {
                    byte itemQuantity = item.Quantity;
                    item.Quantity = 1;
                    for (byte i = 0; i < itemQuantity; i++)
                        Inventory.Add(item);
                }
                else
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
        #endregion
    }
}
