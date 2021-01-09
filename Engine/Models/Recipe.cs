﻿using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Recipe
    {
        public int ID { get; }
        public string Name { get; }
        public List<GroupedItem> Ingredients { get; } = new List<GroupedItem>();
        public List<GroupedItem> OutputItems { get; } = new List<GroupedItem>();

        public string ToolTipContents =>
            "Ingredienti:" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, Ingredients.Select(i => i.GroupedItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Oggetti creati:" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, OutputItems.Select(i => i.GroupedItemDescription));

        public Recipe(int id, string name)
        {
            ID = id;
            Name = name;
        }

        #region Functions
        public void AddIngredient(int itemID, byte quantity)
        {
            if (!Ingredients.Any(x => x.Item.ItemID == itemID))
            {
                Ingredients.Add(new GroupedItem(ItemFactory.GetItemByID(itemID), quantity));
            }
        }

        public void AddOutputItem(int itemID, byte quantity)
        {
            if (!OutputItems.Any(x => x.Item.ItemID == itemID))
            {
                OutputItems.Add(new GroupedItem(ItemFactory.GetItemByID(itemID), quantity));
            }
        }
        #endregion
    }
}
