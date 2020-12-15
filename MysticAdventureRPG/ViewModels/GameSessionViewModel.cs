using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Models;
using Engine.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonClasses.EventArgs;

namespace MysticAdventureRPG.ViewModels
{
    public class GameSessionViewModel : BaseNotifyPropertyChanged
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region Private Properties
        private Location _currentLocation;
        private Enemy _currentEnemy;
        public Weapon _currentWeapon;
        #endregion

        #region Public Properties
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CanMoveForward));
                OnPropertyChanged(nameof(CanMoveRight));
                OnPropertyChanged(nameof(CanMoveBackwards));
                OnPropertyChanged(nameof(CanMoveLeft));
                GivePlayerQuestsAtLocation();
                GetEnemyAtLocation();
            }
        }
        public Enemy CurrentEnemy
        {
            get { return _currentEnemy; }
            set
            {
                _currentEnemy = value;

                OnPropertyChanged(nameof(CurrentEnemy));
                OnPropertyChanged(nameof(HasEnemy));

                if (CurrentEnemy != null)
                {
                    RaiseMessage($"Ti imbatti in un {CurrentEnemy.Name}!", GameMessageType.BattleInfo);
                }
            }
        }
        public Player CurrentPlayer { get; set; }
        public World CurrentWorld { get; set; }
        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                _currentWeapon = value;

                OnPropertyChanged(nameof(CurrentWeapon));
            }
        }
        #endregion

        #region Commands
        public ICommand MoveForwardCommand { get; set; }
        public ICommand MoveBackwardsCommand { get; set; }
        public ICommand MoveRightCommand { get; set; }
        public ICommand MoveLeftCommand { get; set; }
        public ICommand AttackEnemyCommand { get; set; }
        #endregion

        public GameSessionViewModel()
        {
            #region Inizializzazione Comandi
            MoveForwardCommand = new BaseCommand(MoveForward);
            MoveBackwardsCommand = new BaseCommand(MoveBackwards);
            MoveRightCommand = new BaseCommand(MoveRight);
            MoveLeftCommand = new BaseCommand(MoveLeft);
            AttackEnemyCommand = new BaseCommand(EvaluateBattleTurn);
            #endregion

            CurrentPlayer = new Player("Giuseppe", "Penna", PlayerClassType.Mago);
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
            if (CurrentLocation.Name == "Home")
                CurrentLocation.ImageName = $"/Engine;component/Resources/LocationsImages/Home/Home_{CurrentPlayer.Class.ToString()}.jpg";

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1003));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1004));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(4, 5));


        }

        #region Boolean Controls
        public bool CanMoveForward => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool CanMoveRight => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool CanMoveBackwards => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public bool CanMoveLeft => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasEnemy => CurrentEnemy != null;
        #endregion

        #region Functions
        public void MoveForward(object obj)
        {
            if (CanMoveForward)
            {
                CurrentPlayer.YCoordinate++;
                RefreshLocation();
            }
        }

        public void MoveRight(object obj)
        {
            if (CanMoveRight)
            {
                CurrentPlayer.XCoordinate++;
                RefreshLocation();
            }
        }

        public void MoveBackwards(object obj)
        {
            if (CanMoveBackwards)
            {
                CurrentPlayer.YCoordinate--;
                RefreshLocation();
            }
        }

        public void MoveLeft(object obj)
        {
            if (CanMoveLeft)
            {
                CurrentPlayer.XCoordinate--;
                RefreshLocation();
            }
        }

        private void RefreshLocation()
        {
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.QuestID == quest.QuestID))
                {
                    if (quest.Status == QuestStatus.Nuova)
                    {
                        quest.Status = QuestStatus.Iniziata;
                        CurrentPlayer.Quests.Add(quest);
                    }

                }
            }
        }

        private void GetEnemyAtLocation()
        {
            CurrentEnemy = CurrentLocation.GetEnemy();
        }

        private void RaiseMessage(string message, GameMessageType type)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message, type));
        }

        public void EvaluateBattleTurn(object obj)
        {
            if((CurrentPlayer.Speed + CurrentWeapon?.WeaponSpeed) >= CurrentEnemy.Speed)
            {
                EvaluatePlayerTurn();
                if (CurrentEnemy.CurrentHitPoints <= 0)
                {
                    GetLoot();
                    return;
                }
                else
                {
                    EvaluateEnemyTurn();
                    if (CurrentPlayer.CurrentHitPoints <= 0)
                    {
                        PlayerKilled();
                        return;
                    }
                }
            }
            else
            {
                EvaluateEnemyTurn();
                if (CurrentPlayer.CurrentHitPoints <= 0)
                {
                    PlayerKilled();
                    return;
                }
                else
                {
                    EvaluatePlayerTurn();
                    if (CurrentEnemy.CurrentHitPoints <= 0)
                    {
                        GetLoot();
                        return;
                    }
                }
            }
        }

        private void EvaluatePlayerTurn()
        {
            int damageToEnemy;

            // Determino il danno inflitto dal PLayer
            if (CurrentWeapon == null)
            {
                RaiseMessage("Non avendo equipaggiato un'arma ti scagli sul nemico a mani nude.", GameMessageType.BattleInfo);
                damageToEnemy = CurrentPlayer.BaseDamage;
                CurrentEnemy.CurrentHitPoints -= damageToEnemy;
                RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageType.BattlePositive);
            }
            else
            {
                RaiseMessage($"Attacchi il nemico con {CurrentWeapon.Name}.", GameMessageType.BattleInfo);
                if (CurrentWeapon.MissRate > BaseRandomNumberGenerator.NumberBetween(0, 100))
                {
                    RaiseMessage($"Hai mancato {CurrentEnemy.Name}!", GameMessageType.BattleNegative);
                }
                else
                {
                    damageToEnemy = BaseRandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);
                    CurrentEnemy.CurrentHitPoints -= damageToEnemy;
                    RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageType.BattlePositive);
                }
            }
        }

        private void GetLoot()
        {
            // Se il nemico è sconfitto ottengo il loot
            if (CurrentEnemy.CurrentHitPoints <= 0)
            {
                RaiseMessage("", GameMessageType.Info);
                RaiseMessage($"Hai sconfitto {CurrentEnemy.Name}!", GameMessageType.BattleInfo);

                CurrentPlayer.Experience += CurrentEnemy.RewardExperiencePoints;
                RaiseMessage($"Ricevi {CurrentEnemy.RewardExperiencePoints} punti esperienza!", GameMessageType.BattleInfo);

                CurrentPlayer.Gold += CurrentEnemy.RewardGold;
                RaiseMessage($"Ricevi {CurrentEnemy.RewardGold} oro!", GameMessageType.BattleInfo);

                foreach (Item drop in CurrentEnemy.Inventory)
                {
                    Item item = ItemFactory.CreateItem(drop.ItemID, drop.Quantity);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"Ricevi {item.Quantity} {item.Name}!", GameMessageType.BattleInfo);
                }

                // Spawno un altro nemico
                //GetEnemyAtLocation();
            }
        }

        private void EvaluateEnemyTurn()
        {
            //Determino il danno inflitto dal nemico

            if (CurrentEnemy.BaseMissRate > BaseRandomNumberGenerator.NumberBetween(0, 100))
            {
                RaiseMessage($"{CurrentEnemy.Name} ti attacca... ma schivi l'attacco!", GameMessageType.BattlePositive);
            }
            else
            {
                int damageToPlayer = BaseRandomNumberGenerator.NumberBetween(CurrentEnemy.MinimumDamage, CurrentEnemy.MaximumDamage);
                CurrentPlayer.CurrentHitPoints -= damageToPlayer;
                RaiseMessage($"Il {CurrentEnemy.Name} ti colpisce infliggendoti {damageToPlayer} danni!", GameMessageType.BattleNegative);
            }

        }

        private void PlayerKilled()
        {
            // Se il player è sconfitto lo porto al checkpoint e lo curo
            if (CurrentPlayer.CurrentHitPoints <= 0)
            {
                RaiseMessage("", GameMessageType.Info);
                RaiseMessage($"The {CurrentEnemy.Name} killed you.", GameMessageType.BattleNegative);

                CurrentLocation = CurrentWorld.LocationAt(0, -1); // Player's home
                CurrentPlayer.CurrentHitPoints = CurrentPlayer.MaximumHitPoints;
                //CurrentPlayer.CurrentHitPoints = CurrentPlayer.Level * 10; // Completely heal the player
            }
            
        }
        #endregion
    }
}

