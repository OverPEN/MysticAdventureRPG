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
    public abstract class LivingEntity : BaseNotifyPropertyChanged
    {
        #region Private Properties
        private String _name;
        private int _maximumHitPoints;
        private int _currentHitPoints;
        private float _speed;
        private int _gold;
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
        public PlayerClassType Class { get; set; }
        public bool IsDead => CurrentHitPoints <= 0;
        public ObservableCollection<Item> Inventory { get; set; }
        public List<Item> Weapons => Inventory.Where(i => i is Weapon).ToList();
        #endregion

        protected LivingEntity(string name, int maxHitPoints, int currHitPoints, float speed, int gold, PlayerClassType entityClass)
        {
            Name = name;
            MaximumHitPoints = maxHitPoints;
            CurrentHitPoints = currHitPoints;
            Speed = speed;
            Gold = gold;
            Class = entityClass;
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

        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} ha solo {Gold} Oro, non può spenderne {amountOfGold}!" +
                    $"");
            }

            Gold -= amountOfGold;
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
            }
        }

        #endregion
    }
}
