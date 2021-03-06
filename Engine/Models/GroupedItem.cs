﻿using CommonClasses.BaseClasses;
using Newtonsoft.Json;

namespace Engine.Models
{
    public class GroupedItem : BaseNotifyPropertyChanged
    {
        #region Private Properties
        private Item _item;
        private byte _quantity;
        private byte _selectedQuantity;
        #endregion

        #region Public Properties
        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }
        public byte Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        [JsonIgnore]
        public byte SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public string GroupedItemDescription => $"{Quantity} {Item.Name}";
        #endregion

        public GroupedItem(Item item, byte quantity)
        {
            Item = item;
            Quantity = quantity;
            SelectedQuantity = 1;
        }
    }
}
