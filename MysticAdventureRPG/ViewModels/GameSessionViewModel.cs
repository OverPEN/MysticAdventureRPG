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
using MysticAdventureRPG.Views;

namespace MysticAdventureRPG.ViewModels
{
    public class GameSessionViewModel : BaseNotifyPropertyChanged
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region Private Properties
        private Player _currentPlayer;
        private Location _currentLocation;
        private Enemy _currentEnemy;
        public Weapon _currentWeapon;
        private Trader _currentTrader;
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

                RaiseMessage($"Sei giunto a {CurrentLocation.Name}.", GameMessageType.ImportantInfo);
                GivePlayerQuestsAtLocation();
                CompleteQuestsAtLocation();
                GetEnemyAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
                if (HasTrader)
                {
                    RaiseMessage($"Nella zona puoi commerciare con {CurrentTrader.Name}.", GameMessageType.ImportantInfo);
                }            
            }
        }
        public Enemy CurrentEnemy
        {
            get { return _currentEnemy; }
            set
            {
                _currentEnemy = value;

                if (CurrentEnemy != null)
                {
                    RaiseMessage($"Ti imbatti in un {CurrentEnemy.Name}!", GameMessageType.BattleInfo);
                }

                OnPropertyChanged(nameof(CurrentEnemy));
                OnPropertyChanged(nameof(HasEnemy));
            }
        }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
            }
        }
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
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));
            }
        }
        #endregion

        #region Commands
        public ICommand MoveForwardCommand { get; set; }
        public ICommand MoveBackwardsCommand { get; set; }
        public ICommand MoveRightCommand { get; set; }
        public ICommand MoveLeftCommand { get; set; }
        public ICommand AttackEnemyCommand { get; set; }
        public ICommand ShowTraderScreenCommand { get; set; }
        #endregion

        public GameSessionViewModel()
        {
            #region Inizializzazione Comandi
            MoveForwardCommand = new BaseCommand(MoveForward);
            MoveBackwardsCommand = new BaseCommand(MoveBackwards);
            MoveRightCommand = new BaseCommand(MoveRight);
            MoveLeftCommand = new BaseCommand(MoveLeft);
            AttackEnemyCommand = new BaseCommand(EvaluateBattleTurn);
            ShowTraderScreenCommand = new BaseCommand(ShowTraderScreen);
            #endregion

            CurrentPlayer = new Player("Giuseppe Penna", PlayerClassType.Mago);
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
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
        public bool HasTrader => CurrentTrader != null;
        #endregion

        #region Game Session Functions
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

                        RaiseMessage($"Quest attivata: '{quest.Name}'", GameMessageType.ImportantInfo);
                        RaiseMessage(quest.Description, GameMessageType.Info);

                        RaiseMessage("Consegna:", GameMessageType.ImportantInfo);
                        foreach (Item item in quest.ItemsToComplete)
                        {
                            RaiseMessage($"   {item.Quantity} {ItemFactory.CreateItem(item.ItemID).Name}", GameMessageType.Info);
                        }

                        RaiseMessage("Ricompensa:", GameMessageType.Info);
                        RaiseMessage($"   {quest.RewardExperiencePoints} XP", GameMessageType.Info);
                        RaiseMessage($"   {quest.RewardGold} Oro", GameMessageType.Info);
                        foreach (Item item in quest.RewardItems)
                        {
                            RaiseMessage($"   {item.Quantity} {ItemFactory.CreateItem(item.ItemID).Name}", GameMessageType.Info);
                        }
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
                if (CurrentEnemy.IsDead)
                {
                    GetLoot();
                    return;
                }
                else
                {
                    EvaluateEnemyTurn();
                    if (CurrentPlayer.IsDead)
                    {
                        PlayerKilled();
                        return;
                    }
                }
            }
            else
            {
                EvaluateEnemyTurn();
                if (CurrentPlayer.IsDead)
                {
                    PlayerKilled();
                    return;
                }
                else
                {
                    EvaluatePlayerTurn();
                    if (CurrentEnemy.IsDead)
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
                CurrentEnemy.TakeDamage(damageToEnemy);
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
                    CurrentEnemy.TakeDamage(damageToEnemy);
                    RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageType.BattlePositive);
                }
            }
        }

        private void GetLoot()
        {
            // Se il nemico è sconfitto ottengo il loot
            if (CurrentEnemy.CurrentHitPoints <= 0)
            {
                //RaiseMessage("", GameMessageType.Info);
                RaiseMessage($"Hai sconfitto {CurrentEnemy.Name}!", GameMessageType.BattleInfo);

                CurrentPlayer.Experience += CurrentEnemy.RewardExperiencePoints;
                RaiseMessage($"Ricevi {CurrentEnemy.RewardExperiencePoints} punti esperienza!", GameMessageType.BattleInfo);

                CurrentPlayer.Gold += CurrentEnemy.Gold;
                RaiseMessage($"Ricevi {CurrentEnemy.Gold} oro!", GameMessageType.BattleInfo);

                foreach (Item drop in CurrentEnemy.Inventory)
                {
                    Item item = ItemFactory.CreateItem(drop.ItemID, drop.Quantity);
                    CurrentPlayer.AddItemToInventory(item);
                    OnPropertyChanged(nameof(CurrentPlayer.Inventory));
                    RaiseMessage($"Ricevi {item.Quantity} {item.Name}!", GameMessageType.BattleInfo);
                }

                // Spawno un altro nemico
                GetEnemyAtLocation();
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
                RaiseMessage($"Sei stato ucciso da {CurrentEnemy.Name}.", GameMessageType.BattleNegative);

                CurrentLocation = CurrentWorld.GetLocationByID(1); // Player's home
                CurrentPlayer.XCoordinate = CurrentLocation.XCoordinate;
                CurrentPlayer.YCoordinate = CurrentLocation.YCoordinate;
                CurrentPlayer.CompletelyHeal();
                //CurrentPlayer.CurrentHitPoints = CurrentPlayer.Level * 10; // Completely heal the player
            }
            
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                Quest questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.QuestID == quest.QuestID && quest.Status == QuestStatus.Iniziata);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Rimuovo gli oggetti della quest dall'inventario del giocatore
                        foreach (Item item in quest.ItemsToComplete)
                        {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(i => i.ItemID == item.ItemID));
                        }

                        RaiseMessage($"Hai completato la Quest: '{quest.Name}'", GameMessageType.ImportantInfo);

                        // Aggiungo le ricompense della queste al giocatore
                        CurrentPlayer.Experience += quest.RewardExperiencePoints;
                        RaiseMessage($"Hai ricevuto {quest.RewardExperiencePoints} XP", GameMessageType.Info);

                        CurrentPlayer.ReceiveGold(quest.RewardGold);
                        RaiseMessage($"Hai ricevuto {quest.RewardGold} Oro", GameMessageType.Info);

                        foreach (Item item in quest.RewardItems)
                        {
                            Item rewardItem = ItemFactory.CreateItem(item.ItemID,item.Quantity);

                            CurrentPlayer.AddItemToInventory(rewardItem);
                            RaiseMessage($"Hai ricevuto {item.Quantity} {rewardItem.Name}", GameMessageType.Info);
                        }

                        // Segno la Quest come completata
                        quest.Status = QuestStatus.Completata;
                        CurrentPlayer.Quests.Remove(questToComplete);
                    }
                }
            }
        }

        private void ShowTraderScreen(object obj)
        {
            TradeScreen tradeScreen = new TradeScreen();
            tradeScreen.DataContext = this;
            tradeScreen.ShowDialog();
        }
        #endregion
    }
}

