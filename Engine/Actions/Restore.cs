using System;
using CommonClasses.Enums;
using Engine.Models;

namespace Engine.Actions
{
    public class Restore : BaseAction, IAction
    {
        private readonly RestoringItem _item;

        public Restore(RestoringItem itemInUse) : base (itemInUse)
        {
            if (itemInUse.Type != ItemTypeEnum.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} non è un consumabile!");
            }

            _item = itemInUse;
        }

        public void Execute(LivingEntity actor, LivingEntity target, object parameter = null)
        {
            switch (_item.Target)
            {
                case nameof(LivingEntity.CurrentHitPoints):
                    ReportResult($"{actor.Name} si cura di {_item.PointsToRestore} punt{(_item.PointsToRestore > 1 ? "i" : "o")} salute!", GameMessageTypeEnum.BattleInfo);
                    break;
                case nameof(LivingEntity.CurrentStamina):
                    ReportResult($"{actor.Name} ripristina la Stamina di {_item.PointsToRestore} punt{(_item.PointsToRestore > 1 ? "i" : "o")}!", GameMessageTypeEnum.BattleInfo);
                    break;
                case nameof(LivingEntity.CurrentMana):
                    ReportResult($"{actor.Name} ripristina il Mana di {_item.PointsToRestore} punt{(_item.PointsToRestore > 1 ? "i" : "o")}!", GameMessageTypeEnum.BattleInfo);
                    break;
            }
            
            target.Restore(_item.PointsToRestore, _item.Target);
        }
    }
}
