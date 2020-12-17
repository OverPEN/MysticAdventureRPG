using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        #region Public Properties
        public int TraderID { get; set; }
        #endregion

        public Trader(int id, string name)
        {
            TraderID = id;
            Name = name;
            //Inventory = new ObservableCollection<Item>();
        }

    }
}
