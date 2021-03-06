﻿using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotifyPropertyChanged
    {
        public event EventHandler OnKilled;
        public event EventHandler<GameMessageEventArgs> OnBattleActionPerformed;
        public event EventHandler<GameMessageEventArgs> OnConsumableUsed;

        #region Private Properties
        private String _name;
        private int _maximumHitPoints;
        private int _currentHitPoints;
        private float _speed;
        private int _maximumStamina;
        private int _maximumMana;
        private int _currentStamina;
        private int _currentMana;
        private int _gold;
        private Byte _level;
        private Weapon _currentWeapon;
        private Item _currentConsumable;
        #endregion

        #region Public Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public PlayerClassTypeEnum Class { get; }
        public byte Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }
        public int MaximumStamina
        {
            get { return _maximumStamina; }
            set
            {
                _maximumStamina = value;
                OnPropertyChanged();
            }
        }
        public int MaximumMana
        {
            get { return _maximumMana; }
            set
            {
                _maximumMana = value;
                OnPropertyChanged();
            }
        }
        public int CurrentStamina
        {
            get { return _currentStamina; }
            set
            {
                _currentStamina = value;
                OnPropertyChanged();
            }
        }
        public int CurrentMana
        {
            get { return _currentMana; }
            set
            {
                _currentMana = value;
                OnPropertyChanged();
            }
        }
        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseBattleActionPerformedEvent;
                }

                _currentWeapon = value;

                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseBattleActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public bool IsDead => CurrentHitPoints <= 0;
        [JsonIgnore]
        public ObservableCollection<Item> Inventory { get; set; }
        public ObservableCollection<GroupedItem> GroupedInventory { get; set; }
        [JsonIgnore]
        public List<Item> Weapons => Inventory.Where(i => i is Weapon && i.UsableBy.Contains(Class)).ToList();
        [JsonIgnore]
        public List<Item> Consumables => GroupedInventory.Where(i => i.Item.Type == ItemTypeEnum.Consumable && i.Item.UsableBy.Contains(Class)).Select(s=>s.Item).ToList();
        [JsonIgnore]
        public bool HasConsumable => Consumables.Any();
        public Item CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseConsumableUsedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseConsumableUsedEvent;
                }

                OnPropertyChanged();
            }
        }
        #endregion

        protected LivingEntity(string name, int maxHitPoints, int currHitPoints, float speed, int gold, PlayerClassTypeEnum entityClass, int maximumStamina, int maximumMana, int currentStamina, int currentMana, byte level = 1, Weapon currentWeapon = null)
        {
            Name = name;
            MaximumHitPoints = maxHitPoints;
            CurrentHitPoints = currHitPoints;
            Speed = speed;
            MaximumStamina = maximumStamina;
            CurrentStamina = currentStamina;
            MaximumMana = maximumMana;
            CurrentMana = currentMana;
            Gold = gold;
            Class = entityClass;
            Level = level;
            CurrentWeapon = currentWeapon;
            Inventory = new ObservableCollection<Item>();
            GroupedInventory = new ObservableCollection<GroupedItem>();
        }

        #region Functions
        public bool HasAllTheseItems(List<GroupedItem> items)
        {
            foreach (GroupedItem groupedItem in items)
            {
                if (Inventory.FirstOrDefault(i => i.ItemID == groupedItem.Item.ItemID) != null)
                {
                    if (GroupedInventory.FirstOrDefault(i => i.Item.ItemID == groupedItem.Item.ItemID).Quantity < groupedItem.Quantity)
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

        public void AddItemToInventory(GroupedItem groupedItem)
        {
            for(int i=0; i<groupedItem.Quantity; i++)
                Inventory.Add(groupedItem.Item);

            if (groupedItem.Item.IsUnique)
            {
                GroupedInventory.Add(new GroupedItem(groupedItem.Item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemID == groupedItem.Item.ItemID))
                {
                    GroupedInventory.Add(new GroupedItem(groupedItem.Item, 0));
                }

                GroupedInventory.First(gi => gi.Item.ItemID == groupedItem.Item.ItemID).Quantity+= groupedItem.Quantity;
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemFromInventory(GroupedItem groupedItem)
        {
            for (int i = 0; i <groupedItem.Quantity; i++)
                Inventory.Remove(Inventory.FirstOrDefault(f=> f.ItemID == groupedItem.Item.ItemID));

            GroupedItem groupedItemToRemove = GroupedInventory.FirstOrDefault(gi => gi.Item.ItemID == groupedItem.Item.ItemID);


            if (groupedItemToRemove != null)
            {
                if (groupedItemToRemove.Quantity <= 1 || groupedItemToRemove.Quantity <= groupedItem.Quantity)
                {
                    GroupedInventory.Remove(groupedItemToRemove);
                }
                else
                {
                    groupedItemToRemove.Quantity-= groupedItem.Quantity;
                }
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void Restore(int pointsToRestore, string targetProperty)
        {
            switch (targetProperty)
            {
                case nameof(LivingEntity.CurrentHitPoints):
                    CurrentHitPoints += pointsToRestore;
                    if (CurrentHitPoints > MaximumHitPoints)
                        CurrentHitPoints = MaximumHitPoints;
                    break;
                case nameof(LivingEntity.CurrentStamina):
                    CurrentStamina += pointsToRestore;
                    if (CurrentStamina > MaximumStamina)
                        CurrentStamina = MaximumStamina;
                    break;
                case nameof(LivingEntity.CurrentMana):
                    CurrentMana += pointsToRestore;
                    if (CurrentMana > MaximumMana)
                        CurrentMana = MaximumMana;
                    break;
            }
        }

        public void CompletelyRestore(string targetProperty)
        {
            switch (targetProperty)
            {
                case nameof(LivingEntity.CurrentHitPoints):
                    CurrentHitPoints = MaximumHitPoints;
                    break;
                case nameof(LivingEntity.CurrentStamina):
                    CurrentStamina = MaximumStamina;
                    break;
                case nameof(LivingEntity.CurrentMana):
                    CurrentMana += MaximumMana;
                    break;
                default:
                    CurrentHitPoints = MaximumHitPoints;
                    CurrentStamina = MaximumStamina;
                    CurrentMana += MaximumMana;
                    break;
            }           
        }

        public void LoseStaminaOrMana(int pointsToLose)
        {
            if (Class != PlayerClassTypeEnum.Mago)
            {
                CurrentStamina -= pointsToLose;
                CurrentStamina = CurrentStamina < 0 ? 0 : CurrentStamina;
            }
            else
            {
                CurrentMana -= pointsToLose;
                CurrentMana = CurrentMana < 0 ? 0 : CurrentMana;
            }               
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
                RaiseOnKilledEvent();
            }
        }

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        private void RaiseBattleActionPerformedEvent(object sender, GameMessageEventArgs result)
        {
            OnBattleActionPerformed?.Invoke(this, result);
        }

        private void RaiseConsumableUsedEvent(object sender, GameMessageEventArgs result)
        {
            OnConsumableUsed?.Invoke(this, result);
        }

        public void UseCurrentWeaponOn(LivingEntity target, string attackType = "L")
        {
            if(target!=null)
                CurrentWeapon.PerformAction(this, target, attackType);
        }

        public void UseCurrentConsumable()
        {
            if (CurrentConsumable != null)
            {
                CurrentConsumable.PerformAction(this, this);
                RemoveItemFromInventory(new GroupedItem(CurrentConsumable, 1));
            }
        }

        #endregion
    }
}
