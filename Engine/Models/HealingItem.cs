using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class HealingItem : Item
    {
        public int HitPointsToHeal { get; set; }

        public HealingItem(int itemID, string name, int price, int hitPointsToHeal) : base(itemID, name, price, ItemTypeEnum.Consumable)
        {
            HitPointsToHeal = hitPointsToHeal;
        }
    }
}
