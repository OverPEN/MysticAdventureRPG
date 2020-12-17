using CommonClasses.BaseClasses;
using CommonClasses.Enums;
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
        private byte _quantity;
        [XmlIgnore]
        private  byte _selectedQuantity;

        public int ItemID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public byte Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        [XmlIgnore]
        public byte SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                OnPropertyChanged(nameof(SelectedQuantity));
            }
        }

        public Item(int itemID, string name, int price, ItemType type, byte quantity = 1, byte selectedQuantity = 1)
        {
            ItemID = itemID;
            Name = name;
            Price = price;
            Type = type;
            Quantity = quantity;
            SelectedQuantity = selectedQuantity;
        }

        public Item Clone(byte quantity = 1)
        {
            return new Item(ItemID, Name, Price, Type, Quantity = quantity);
        }
    }
}
