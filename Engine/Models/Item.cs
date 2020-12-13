using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public byte Quantity { get; set; }

        public Item(int itemID, string name, int price, ItemType type, byte quantity = 1)
        {
            ItemID = itemID;
            Name = name;
            Price = price;
            Type = type;
            Quantity = quantity;
        }

        public Item Clone(byte quantity = 1)
        {
            return new Item(ItemID, Name, Price, Type, Quantity = quantity);
        }
    }
}
