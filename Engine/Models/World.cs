using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Location> Locations = new ObservableCollection<Location>();

        #region Functions
        internal void AddLocation(Location location)
        {
            if (Locations.FirstOrDefault(w => w.LocationID == location.LocationID) == null)
                Locations.Add(location);
            else
                throw new InvalidOperationException($"La location {location.Name} è già presente!");
        }

        public Location LocationAt(int xCoord, int yCoord)
        {
            Location loc = Locations.FirstOrDefault(f => f.XCoordinate == xCoord && f.YCoordinate == yCoord);

            return loc;
        }

        public Location GetLocationByID(int id)
        {
            Location loc = Locations.FirstOrDefault(f => f.LocationID == id);

            return loc;
        }
        #endregion
    }
}
