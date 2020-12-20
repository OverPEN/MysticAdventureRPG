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
        #region Private Properties
        private byte _quantity;
        [XmlIgnore]
        private  byte _selectedQuantity;
        #endregion

        #region Public Properties
        public int ItemID { get; }
        public string Name { get; }
        public int Price { get; }
        public ItemType Type { get; }
        public byte Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public byte SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                OnPropertyChanged();
            }
        }
        public bool IsUnique { get; }
        public IAction Action { get; set; }

        #endregion

        public Item(int itemID, string name, int price, ItemType type, byte quantity = 1, byte selectedQuantity = 1, bool isUnique = false, IAction action = null)
        {
            ItemID = itemID;
            Name = name;
            Price = price;
            Type = type;
            Quantity = quantity;
            SelectedQuantity = selectedQuantity;
            IsUnique = isUnique;
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }
        public Item Clone(byte quantity = 1)
        {
            return new Item(ItemID, Name, Price, Type, Quantity = quantity, 1, IsUnique);
        }
    }
}
