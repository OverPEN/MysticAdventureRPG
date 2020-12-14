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

namespace MysticAdventureRPG.ViewModels
{
    public class GameSessionViewModel : BaseNotifyPropertyChanged
    {
        private Location _currentLocation;
        private Enemy _currentEnemy;

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
            }
        }
        public Player CurrentPlayer { get; set; }
        public World CurrentWorld { get; set; }

        #region Commands
        public ICommand MoveForwardCommand { get; set; }
        public ICommand MoveBackwardsCommand { get; set; }
        public ICommand MoveRightCommand { get; set; }
        public ICommand MoveLeftCommand { get; set; }
        #endregion

        public GameSessionViewModel()
        {
            MoveForwardCommand = new BaseCommand(MoveForward);
            MoveBackwardsCommand = new BaseCommand(MoveBackwards);
            MoveRightCommand = new BaseCommand(MoveRight);
            MoveLeftCommand = new BaseCommand(MoveLeft);

            CurrentPlayer = new Player("Giuseppe","Penna",PlayerClassType.Mago);
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
            if(CurrentLocation.Name == "Home")
                CurrentLocation.ImageName = $"/Engine;component/Resources/LocationsImages/Home/Home_{CurrentPlayer.Class.ToString()}.jpg";
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(1001));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(1002));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(1003));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(1004));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(4,5));
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
                    if(quest.Status == QuestStatus.Nuova)
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
        #endregion
    }
}
