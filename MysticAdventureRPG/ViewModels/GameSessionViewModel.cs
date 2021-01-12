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
using Services;

namespace MysticAdventureRPG.ViewModels
{
    public class GameSessionViewModel : BaseNotifyPropertyChanged
    {

        #region Private Properties
        private Player _currentPlayer;
        private Location _currentLocation;
        private Enemy _currentEnemy;
        private Trader _currentTrader;
        private readonly Dictionary<Key, ICommand> _userInputActions = new Dictionary<Key, ICommand>();
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

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

                _messageBroker.RaiseMessage($"Sei giunto a {CurrentLocation.Name}.", GameMessageTypeEnum.ImportantInfo);
                GivePlayerQuestsAtLocation();
                CompleteQuestsAtLocation();
                GetEnemyAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
                if (HasTrader)
                {
                    _messageBroker.RaiseMessage($"Nella zona puoi commerciare con {CurrentTrader.Name}.", GameMessageTypeEnum.ImportantInfo);
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
                    _currentEnemy.OnActionPerformed -= OnCurrentEnemyPerformedAction;
                    _currentEnemy.OnKilled -= OnCurrentEnemyKilled;
                }

                _currentEnemy = value;

                if (CurrentEnemy != null)
                {
                    _currentEnemy.OnActionPerformed += OnCurrentEnemyPerformedAction;
                    _currentEnemy.OnKilled += OnCurrentEnemyKilled;
                    _messageBroker.RaiseMessage($"Ti imbatti in un {CurrentEnemy.Name}!", GameMessageTypeEnum.BattleInfo);
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
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }
                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }
        public World CurrentWorld { get; set; }
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
        public ICommand UseCurrentConsumableCommand { get; set; }
        public ICommand SetTabFocusToCommand { get; set; }
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
            UseCurrentConsumableCommand = new BaseCommand(UseCurrentConsumable);
            SetTabFocusToCommand = new BaseCommand(SetTabFocusTo);
            #endregion

            InitializeUserInputActions();

            CurrentPlayer = new Player("Giuseppe Penna", PlayerClassTypeEnum.Guerriero);
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);

            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(1001));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(1002));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(1003));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(1004));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(4, 5));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(1005));
            CurrentPlayer.AddItemToInventory(ItemFactory.ObtainItem(2001, 3));

            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
        }

        #region Boolean Controls
        public bool CanMoveForward => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool CanMoveRight => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool CanMoveBackwards => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public bool CanMoveLeft => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasEnemy => CurrentEnemy != null;
        public bool HasTrader => CurrentTrader != null;

        private bool wasKilled = false;
        #endregion

        #region Functions From View
        private void MoveForward(object obj)
        {
            if (CanMoveForward)
            {
                CurrentPlayer.YCoordinate++;
                RefreshLocation();
            }
        }

        private void MoveRight(object obj)
        {
            if (CanMoveRight)
            {
                CurrentPlayer.XCoordinate++;
                RefreshLocation();
            }
        }

        private void MoveBackwards(object obj)
        {
            if (CanMoveBackwards)
            {
                CurrentPlayer.YCoordinate--;
                RefreshLocation();
            }
        }

        private void MoveLeft(object obj)
        {
            if (CanMoveLeft)
            {
                CurrentPlayer.XCoordinate--;
                RefreshLocation();
            }
        }

        private void EvaluateBattleTurn(object obj)
        {
            if (HasEnemy)
            {
                if ((CurrentPlayer.Speed + CurrentPlayer.CurrentWeapon?.WeaponSpeed) >= (CurrentEnemy.Speed + CurrentEnemy.CurrentWeapon.WeaponSpeed))
                {
                    EvaluatePlayerTurn();
                    if (CurrentEnemy.IsDead)
                    {
                        GetEnemyAtLocation();
                    }
                    else
                    {
                        CurrentEnemy.UseCurrentWeaponOn(CurrentPlayer);
                    }
                }
                else
                {
                    CurrentEnemy.UseCurrentWeaponOn(CurrentPlayer);
                    if (wasKilled)
                    {
                        wasKilled = false;
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
        }

        private void ShowTraderScreen(object obj)
        {
            if(CurrentTrader!= null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.DataContext = this;
                tradeScreen.ShowDialog();
            }
            
        }

        private void UseCurrentConsumable(object obj)
        {
            CurrentPlayer.UseCurrentConsumable();
        }

        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.HasAllTheseItems(recipe.Ingredients))
            {
                foreach(GroupedItem groupedItem in recipe.Ingredients)
                {
                    CurrentPlayer.RemoveItemFromInventory(groupedItem);
                }
               
                foreach (GroupedItem groupedItem in recipe.OutputItems)
                {
                        GroupedItem outputItem = ItemFactory.ObtainItem(groupedItem.Item.ItemID, groupedItem.Quantity);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"Hai creato {groupedItem.Quantity} {outputItem.Item.Name}", GameMessageTypeEnum.Info);
                }
            }
            else
            {
                _messageBroker.RaiseMessage("Non hai gli ingredienti necessari:", GameMessageTypeEnum.ImportantInfo);
                foreach (GroupedItem groupedItem in recipe.Ingredients)
                {
                    _messageBroker.RaiseMessage($"  {groupedItem.Quantity} {groupedItem.Item.Name}", GameMessageTypeEnum.Info);
                }
            }
        }

        public void ExecuteFromKeyboard( Key key, ref TabControl tabControl)
        {
            if (_userInputActions.ContainsKey(key))
            {
                if(key == Key.Q || key == Key.I || key == Key.R)
                {
                    Tuple<TabControl, string> parameters = new Tuple<TabControl, string>(tabControl, key == Key.Q ? "Quests" : key == Key.I ? "Inventory" : "Crafting");
                    _userInputActions[key].Execute(parameters);
                }      
                else
                    _userInputActions[key].Execute(null);
            }
        }
        #endregion

        #region Private Functions
        private void RefreshLocation()
        {
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (QuestStatus questStatus in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.Quest.QuestID == questStatus.Quest.QuestID))
                {
                    if (questStatus.Status == QuestStatusEnum.Nuova)
                    {
                        questStatus.Status = QuestStatusEnum.Iniziata;
                        
                        _messageBroker.RaiseMessage($"Quest attivata: '{questStatus.Quest.Name}'", GameMessageTypeEnum.ImportantInfo);
                        _messageBroker.RaiseMessage(questStatus.Quest.Description, GameMessageTypeEnum.Info);

                        _messageBroker.RaiseMessage("Consegna:", GameMessageTypeEnum.ImportantInfo);
                        foreach (GroupedItem groupedItem in questStatus.Quest.ItemsToComplete)
                        {
                            _messageBroker.RaiseMessage($"   {groupedItem.Quantity} {groupedItem.Item.Name}", GameMessageTypeEnum.Info);
                        }

                        _messageBroker.RaiseMessage("Ricompensa:", GameMessageTypeEnum.Info);
                        _messageBroker.RaiseMessage($"   {questStatus.Quest.RewardExperiencePoints} XP", GameMessageTypeEnum.Info);
                        _messageBroker.RaiseMessage($"   {questStatus.Quest.RewardGold} Oro", GameMessageTypeEnum.Info);
                        foreach (GroupedItem groupedItem in questStatus.Quest.RewardItems)
                        {
                            _messageBroker.RaiseMessage($"   {groupedItem.Quantity} {groupedItem.Item.Name}", GameMessageTypeEnum.Info);
                        }

                        CurrentPlayer.Quests.Add(questStatus);
                    }

                }
            }
        }

        private void GetEnemyAtLocation()
        {
            
            CurrentEnemy = CurrentLocation.GetEnemyAtLocation();
        }

        private void EvaluatePlayerTurn()
        {
            int damageToEnemy;

            // Determino il danno inflitto dal PLayer
            if (CurrentPlayer.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("Non avendo equipaggiato un'arma ti scagli sul nemico a mani nude.", GameMessageTypeEnum.BattleInfo);
                damageToEnemy = CurrentPlayer.BaseDamage;
                _messageBroker.RaiseMessage($"Hai colpito {CurrentEnemy.Name} causando {damageToEnemy} danni!", GameMessageTypeEnum.BattlePositive);
                CurrentEnemy.TakeDamage(damageToEnemy);
            }
            else
            {
                CurrentPlayer.UseCurrentWeaponOn(CurrentEnemy);
            }
        }

        private void OnCurrentEnemyKilled(object sender, EventArgs eventArgs)
        {
            // Se il nemico è sconfitto ottengo il loot
                _messageBroker.RaiseMessage($"Hai sconfitto {CurrentEnemy.Name}!", GameMessageTypeEnum.BattleInfo);

                _messageBroker.RaiseMessage($"Ricevi {CurrentEnemy.RewardExperiencePoints} punti esperienza!", GameMessageTypeEnum.BattleInfo);
                CurrentPlayer.AddExperience(CurrentEnemy.RewardExperiencePoints);

                _messageBroker.RaiseMessage($"Ricevi {CurrentEnemy.Gold} oro!", GameMessageTypeEnum.BattleInfo);
                CurrentPlayer.ReceiveGold(CurrentEnemy.Gold);

                foreach (GroupedItem drop in CurrentEnemy.GroupedInventory)
                {
                    _messageBroker.RaiseMessage($"Ricevi {drop.Quantity} {drop.Item.Name}!", GameMessageTypeEnum.BattleInfo);
                    CurrentPlayer.AddItemToInventory(drop);
                    OnPropertyChanged(nameof(CurrentPlayer.Inventory));
                    OnPropertyChanged(nameof(CurrentPlayer.GroupedInventory));
                    foreach(QuestStatus questStatus in CurrentPlayer.Quests)
                    {
                        if(questStatus.Quest.ItemsToComplete.Exists(e=>e.Item.ItemID == drop.Item.ItemID))
                        {
                        if (CurrentPlayer.HasAllTheseItems(questStatus.Quest.ItemsToComplete))
                            questStatus.Status = QuestStatusEnum.Completabile;
                        }
                    }

                }
        }

        private void OnCurrentPlayerKilled(object sender, EventArgs eventArgs)
        {
            // Se il player è sconfitto lo porto al checkpoint e lo curo
                _messageBroker.RaiseMessage($"Sei stato ucciso.", GameMessageTypeEnum.BattleNegative);

                CurrentLocation = CurrentWorld.GetLocationByID(1); // Player's home
                CurrentPlayer.XCoordinate = CurrentLocation.XCoordinate;
                CurrentPlayer.YCoordinate = CurrentLocation.YCoordinate;
                CurrentPlayer.CompletelyHeal();
                wasKilled = true;
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (QuestStatus questStatus in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.Quest.QuestID == questStatus.Quest.QuestID && questStatus.Status == QuestStatusEnum.Completabile);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(questStatus.Quest.ItemsToComplete))
                    {
                        _messageBroker.RaiseMessage($"Hai completato la Quest: '{questStatus.Quest.Name}'", GameMessageTypeEnum.ImportantInfo);

                        // Rimuovo gli oggetti della quest dall'inventario del giocatore
                        foreach (GroupedItem groupedItem in questStatus.Quest.ItemsToComplete)
                        {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.GroupedInventory.First(i => i.Item.ItemID == groupedItem.Item.ItemID));
                        }

                        // Aggiungo le ricompense della queste al giocatore
                        _messageBroker.RaiseMessage($"Hai ricevuto {questStatus.Quest.RewardExperiencePoints} XP", GameMessageTypeEnum.Info);
                        CurrentPlayer.AddExperience(questStatus.Quest.RewardExperiencePoints);

                        _messageBroker.RaiseMessage($"Hai ricevuto {questStatus.Quest.RewardGold} Oro", GameMessageTypeEnum.Info);
                        CurrentPlayer.ReceiveGold(questStatus.Quest.RewardGold);

                        foreach (GroupedItem rewardItem in questStatus.Quest.RewardItems)
                        {
                            _messageBroker.RaiseMessage($"Hai ricevuto {rewardItem.Quantity} {rewardItem.Item.Name}", GameMessageTypeEnum.Info);
                            CurrentPlayer.AddItemToInventory(rewardItem);

                        }

                        // Segno la Quest come completata
                        questStatus.Status = QuestStatusEnum.Completata;
                        CurrentPlayer.Quests.Remove(questToComplete);
                    }
                }
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage($"Hai raggiunto il Livello {CurrentPlayer.Level}!", GameMessageTypeEnum.BattlePositive);
        }

        private void OnCurrentPlayerPerformedAction(object sender, GameMessageEventArgs e)
        {
            _messageBroker.RaiseMessage(e.Message, e.Type);
        }

        private void OnCurrentEnemyPerformedAction(object sender, GameMessageEventArgs e)
        {
            _messageBroker.RaiseMessage(e.Message, e.Type);
        }

        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, MoveForwardCommand);
            _userInputActions.Add(Key.A, MoveLeftCommand);
            _userInputActions.Add(Key.S, MoveBackwardsCommand);
            _userInputActions.Add(Key.D, MoveRightCommand);
            _userInputActions.Add(Key.Z, AttackEnemyCommand);
            _userInputActions.Add(Key.C, UseCurrentConsumableCommand);
            _userInputActions.Add(Key.I, SetTabFocusToCommand);
            _userInputActions.Add(Key.Q, SetTabFocusToCommand);
            _userInputActions.Add(Key.R, SetTabFocusToCommand);
            _userInputActions.Add(Key.T, ShowTraderScreenCommand);
        }

        private void SetTabFocusTo(object obj)
        {
            Tuple<TabControl, string> parameter = obj as Tuple<TabControl, string>;
            foreach (object item in parameter.Item1.Items)
            {
                if (item is TabItem tabItem)
                {
                    if (tabItem.Name == parameter.Item2)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }

        #endregion
    }
}

