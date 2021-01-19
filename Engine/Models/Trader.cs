using CommonClasses.Enums;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        #region Public Properties
        public int TraderID { get; set; }
        #endregion

        public Trader(int id, string name) : base(name,int.MaxValue,int.MaxValue,1f, int.MaxValue, PlayerClassTypeEnum.Trader)
        {
            TraderID = id;
        }

    }
}
