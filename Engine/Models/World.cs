using CommonClasses.BaseClasses;
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

        internal void AddLocation(Location location)
        {
            if (!_locations.Exists(w => w.LocationID == location.LocationID))
                _locations.Add(location);
            else
                throw new InvalidOperationException($"La location {location.Name} è già presente!");
        }

        public Location LocationAt(int xCoord, int yCoord)
        {
            Location loc = _locations.FirstOrDefault(f => f.XCoordinate == xCoord && f.YCoordinate == yCoord);

            return loc;
        }

        public Location GetLocationByID(int id)
        {
            Location loc = _locations.FirstOrDefault(f => f.LocationID == id);

            return loc;
        }
    }
}
