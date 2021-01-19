using CommonClasses.Enums;

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
