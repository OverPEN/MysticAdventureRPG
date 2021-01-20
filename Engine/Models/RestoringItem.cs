using CommonClasses.Enums;

namespace Engine.Models
{
    public class RestoringItem : Item
    {
        public int PointsToRestore { get; set; }
        public string Target { get; set; }

        public RestoringItem(int itemID, string name, int price, int pointsToRestore, string target) : base(itemID, name, price, ItemTypeEnum.Consumable)
        {
            PointsToRestore = pointsToRestore;
            Target = target;
        }
    }
}
