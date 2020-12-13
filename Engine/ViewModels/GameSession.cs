using CommonClasses.Enums;
using Engine.Factories;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Location _currentLocation;

        public Player CurrentPlayer { get; set; }
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
        public World CurrentWorld { get; set; }

        public GameSession()
        {
            CurrentPlayer = new Player("Giuseppe","Penna",PlayerClassType.Mago);
            WorldFactory factory = new WorldFactory();
            CurrentWorld = factory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(CurrentPlayer.XCoordinate, CurrentPlayer.YCoordinate);
            if(CurrentLocation.Name == "Home")
                CurrentLocation.ImageName = $"/Engine;component/Resources/LocationsImages/Home/Home_{CurrentPlayer.Class.ToString()}.jpg";
        }
        public void MoveForward()
        {
            CurrentPlayer.YCoordinate++;
            RefreshLocation();
        }

        public void MoveRight()
        {
            CurrentPlayer.XCoordinate++;
            RefreshLocation();
        }

        public void MoveBackwords()
        {
            CurrentPlayer.YCoordinate--;
            RefreshLocation();
        }

        public void MoveLeft()
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
