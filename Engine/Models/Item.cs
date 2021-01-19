using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Actions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Item : BaseNotifyPropertyChanged
    {
        #region Public Properties
        public int ItemID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public int Price { get; }
        [JsonIgnore]
        public ItemTypeEnum Type { get; }
        [JsonIgnore]
        public bool IsUnique { get; }
        [JsonIgnore]
        public IAction Action { get; set; }
        [JsonIgnore]
        public List<PlayerClassTypeEnum> UsableBy = new List<PlayerClassTypeEnum>();
        #endregion

        public Item(int itemID, string name, int price, ItemTypeEnum type, bool isUnique = false, IAction action = null, List<PlayerClassTypeEnum> usableBy = null)
        {
            ItemID = itemID;
            Name = name;
            Price = price;
            Type = type;
            IsUnique = isUnique;
            Action = action;
            UsableBy = usableBy;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }
        public Item Clone()
        {
            return new Item(ItemID, Name, Price, Type, IsUnique, Action, UsableBy);
        }
    }
}
