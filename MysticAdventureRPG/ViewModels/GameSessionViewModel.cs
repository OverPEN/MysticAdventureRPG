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
    public class GameSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                OnPropertyChanged("CurrentLocation");
                OnPropertyChanged("CanMoveForward");
                OnPropertyChanged("CanMoveRight");
                OnPropertyChanged("CanMoveBackwards");
                OnPropertyChanged("CanMoveLeft");
            }
        }
        public ICommand MoveForwardCommand { get; set; }
        public ICommand MoveBackwardsCommand { get; set; }
        public ICommand MoveRightCommand { get; set; }
        public ICommand MoveLeftCommand { get; set; }
        public Player CurrentPlayer { get; set; }
        public World CurrentWorld { get; set; }

        public GameSessionViewModel()
        {
            MoveForwardCommand = new BaseCommand(MoveForward);
            MoveBackwardsCommand = new BaseCommand(MoveBackwards);
            MoveRightCommand = new BaseCommand(MoveRight);
            MoveLeftCommand = new BaseCommand(MoveLeft);

            CurrentPlayer = new Player("Giuseppe","Penna",PlayerClassType.Mago);
            WorldFactory factory = new WorldFactory();
            CurrentWorld = factory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
            if(CurrentLocation.Name == "Home")
                CurrentLocation.ImageName = $"/Engine;component/Resources/LocationsImages/Home/Home_{CurrentPlayer.Class.ToString()}.jpg";
        }

        public void MoveForward(object obj)
        {
            CurrentPlayer.YCoordinate++;
            RefreshLocation();
        }

        public void MoveRight(object obj)
        {
            CurrentPlayer.XCoordinate++;
            RefreshLocation();
        }

        public void MoveBackwards(object obj)
        {
            CurrentPlayer.YCoordinate--;
            RefreshLocation();
        }

        public void MoveLeft(object obj)
        {
            CurrentPlayer.XCoordinate--;
            RefreshLocation();
        }

        private void RefreshLocation()
        {
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
        }

        public bool CanMoveForward
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
            }
        }

        public bool CanMoveRight
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
            }
        }

        public bool CanMoveBackwards
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
            }
        }

        public bool CanMoveLeft
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
