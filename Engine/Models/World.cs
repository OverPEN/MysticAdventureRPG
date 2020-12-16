﻿using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World : BaseNotifyPropertyChanged
    {
        private int _worldID { get; set; }
        private List<Location> _locations = new List<Location>();

        public int WorldID
        {
            get { return _worldID; }
            set
            {
                _worldID = value;
                OnPropertyChanged(nameof(WorldID));
            }
        }

        internal void AddLocation(int id, int xCoordinate, int yCoordinate, string name, string description)
        {
            Location loc = new Location();
            loc.LocationID = id;
            loc.XCoordinate = xCoordinate;
            loc.YCoordinate = yCoordinate;
            loc.Name = name.Replace('_', ' ');
            loc.Description = description;
            loc.ImageName = $"/Engine;component/Resources/LocationsImages/Home/Home_{name}.jpg";

            _locations.Add(loc);
        }

        public Location LocationAt(int xCoord, int yCoord)
        {
            Location loc = _locations.FirstOrDefault(f => f.XCoordinate == xCoord && f.YCoordinate == yCoord);

            return loc;
        }
    }
}
