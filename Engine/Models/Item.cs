using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.Models
{
    public class Item : BaseNotifyPropertyChanged
    {
        #region Public Properties
        public int ItemID { get; }
        public string Name { get; }
        public int Price { get; }
        public ItemType Type { get; }
        public bool IsUnique { get; }
        public IAction Action { get; set; }
        #endregion

        public Item(int itemID, string name, int price, ItemType type, bool isUnique = false, IAction action = null)
        {
            ItemID = itemID;
            Name = name;
            Price = price;
            Type = type;
            IsUnique = isUnique;
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }
        public Item Clone()
        {
            return new Item(ItemID, Name, Price, Type, IsUnique, Action);
        }
    }
}
