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
        public int TraderID { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }

        public Trader(int id, string name)
        {
            TraderID = id;
            Name = name;
            Inventory = new ObservableCollection<Item>();
        }

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
                    Inventory.Remove(item);
                }
                else
                {
                    Inventory.FirstOrDefault(i => i.ItemID == item.ItemID).Quantity -= item.Quantity;
                }
            }
        }
    }
}
