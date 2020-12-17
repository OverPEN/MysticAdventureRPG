using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Trader : BaseNotifyPropertyChanged
    {
        #region Public Properties
        public int TraderID { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }
        #endregion

        public Trader(int id, string name)
        {
            TraderID = id;
            Name = name;
            Inventory = new ObservableCollection<Item>();
        }

        #region Functions
        public void AddItemToInventory(Item item)
        {
            if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) == null)
            {
                Inventory.Add(item);
            }
            else
            {
                Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity += item.Quantity;
            }
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID) != null)
            {
                if (Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity <= item.Quantity)
                {
                    Inventory.Remove(Inventory.FirstOrDefault(i => i.ItemID == item.ItemID));
                }
                else
                {
                    Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity -= item.Quantity;
                }
            }
        }
        #endregion
    }
}
