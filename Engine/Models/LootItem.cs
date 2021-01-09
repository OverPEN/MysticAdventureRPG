using CommonClasses.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class LootItem
    {
        public int ItemID { get; }
        public int DropRate { get; }
        public byte Quantity { get; }

        public LootItem(int itemID, int dropRate, int minQ, int maxQ)
        {
            ItemID = itemID;
            DropRate = dropRate;
            Quantity = (byte)BaseRandomNumberGenerator.NumberBetween(minQ, maxQ);
        }
    }
}
