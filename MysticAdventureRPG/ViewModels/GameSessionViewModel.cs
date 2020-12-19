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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

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

                OnPropertyChanged();
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
                if (_currentEnemy != null)
                {
                    _currentEnemy.OnKilled -= OnCurrentEnemyKilled;
                }

                _currentEnemy = value;

                if (CurrentEnemy != null)
                {
                    _currentEnemy.OnKilled += OnCurrentEnemyKilled;
                    RaiseMessage($"Ti imbatti in un {CurrentEnemy.Name}!", GameMessageType.BattleInfo);
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasEnemy));
            }
        }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }
                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }
        public World CurrentWorld { get; set; }
        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                _currentWeapon = value;

                OnPropertyChanged();
            }
        }
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
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

            CurrentPlayer = new Player("Giuseppe Penna", PlayerClassType.Guerriero);
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1003));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1004));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(4, 5));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1001));
        }

        #region Boolean Controls
        public bool CanMoveForward => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool CanMoveRight => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool CanMoveBackwards => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public bool CanMoveLeft => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasEnemy => CurrentEnemy != null;
        public bool HasTrader => CurrentTrader != null;
        #endregion

        #region Public Functions From View
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

        public void OnGameMessageRaised(object sender, GameMessageEventArgs e, ref RichTextBox gameConsole)
        {
            Paragraph separator = new Paragraph(new Run(""));
            separator.FontSize = 3f;
            Paragraph par = new Paragraph(new Run(e.Message));
            switch (e.Type)
            {
                case GameMessageType.Info:
                    par.Foreground = Brushes.DimGray;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontStyle = FontStyles.Italic;
                    par.FontSize = 12f;
                    break;
                case GameMessageType.ImportantInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 12.5f;
                    par.TextDecorations = TextDecorations.Underline;
                    break;
                case GameMessageType.BattleInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageType.BattleNegative:
                    par.Foreground = Brushes.Red;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageType.BattlePositive:
                    par.Foreground = Brushes.ForestGreen;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
            }
            gameConsole.Document.Blocks.Add(separator);
            gameConsole.Document.Blocks.Add(par);
            gameConsole.ScrollToEnd();
        }
        #endregion

        #region Private Functions
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
                if (CurrentEnemy.IsDead)
                {
                    GetEnemyAtLocation();
                }
                else
                {
                    EvaluateEnemyTurn();
                }
            }
            else
            {
                EvaluateEnemyTurn();
                if (CurrentPlayer.IsDead)
                {
                    CurrentPlayer.CompletelyHeal();
                    return;
                }
                else
                {
                    EvaluatePlayerTurn();
                    if (CurrentEnemy.IsDead)
                    {
                        GetEnemyAtLocation();
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
                RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageType.BattlePositive);
                CurrentEnemy.TakeDamage(damageToEnemy);
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
                    RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageType.BattlePositive);
                    CurrentEnemy.TakeDamage(damageToEnemy);
                }
            }
        }

        private void OnCurrentEnemyKilled(object sender, EventArgs eventArgs)
        {
            // Se il nemico è sconfitto ottengo il loot
                RaiseMessage($"Hai sconfitto {CurrentEnemy.Name}!", GameMessageType.BattleInfo);

                RaiseMessage($"Ricevi {CurrentEnemy.RewardExperiencePoints} punti esperienza!", GameMessageType.BattleInfo);
                CurrentPlayer.AddExperience(CurrentEnemy.RewardExperiencePoints);

                RaiseMessage($"Ricevi {CurrentEnemy.Gold} oro!", GameMessageType.BattleInfo);
                CurrentPlayer.ReceiveGold(CurrentEnemy.Gold);

                foreach (Item drop in CurrentEnemy.Inventory)
                {
                    Item item = ItemFactory.CreateItem(drop.ItemID, drop.Quantity);
                    RaiseMessage($"Ricevi {item.Quantity} {item.Name}!", GameMessageType.BattleInfo);
                    CurrentPlayer.AddItemToInventory(item);
                    OnPropertyChanged(nameof(CurrentPlayer.Inventory));
                    
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
                RaiseMessage($"Il {CurrentEnemy.Name} ti colpisce infliggendoti {damageToPlayer} danni!", GameMessageType.BattleNegative);
                CurrentPlayer.TakeDamage(damageToPlayer);             
            }
        }

        private void OnCurrentPlayerKilled(object sender, EventArgs eventArgs)
        {
            // Se il player è sconfitto lo porto al checkpoint e lo curo
                RaiseMessage($"Sei stato ucciso da {CurrentEnemy.Name}.", GameMessageType.BattleNegative);

                CurrentLocation = CurrentWorld.GetLocationByID(1); // Player's home
                CurrentPlayer.XCoordinate = CurrentLocation.XCoordinate;
                CurrentPlayer.YCoordinate = CurrentLocation.YCoordinate;   
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
                        RaiseMessage($"Hai completato la Quest: '{quest.Name}'", GameMessageType.ImportantInfo);

                        // Rimuovo gli oggetti della quest dall'inventario del giocatore
                        foreach (Item item in quest.ItemsToComplete)
                        {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(i => i.ItemID == item.ItemID));
                        }

                        // Aggiungo le ricompense della queste al giocatore
                        RaiseMessage($"Hai ricevuto {quest.RewardExperiencePoints} XP", GameMessageType.Info);
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"Hai ricevuto {quest.RewardGold} Oro", GameMessageType.Info);
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (Item item in quest.RewardItems)
                        {
                            Item rewardItem = ItemFactory.CreateItem(item.ItemID,item.Quantity);

                            RaiseMessage($"Hai ricevuto {item.Quantity} {rewardItem.Name}", GameMessageType.Info);
                            CurrentPlayer.AddItemToInventory(rewardItem);
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

        private void OnCurrentPlayerLeveledUp(object sender, EventArgs eventArgs)
        {
            RaiseMessage($"Hai raggiunto il Livello {CurrentPlayer.Level}!", GameMessageType.BattlePositive);
        }
        #endregion
    }
}

