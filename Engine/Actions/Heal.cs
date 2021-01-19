using System;
using CommonClasses.Enums;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly HealingItem _item;
        public readonly int _hitPointsToHeal;

        public Heal(HealingItem itemInUse) : base (itemInUse)
        {
            if (itemInUse.Type != ItemTypeEnum.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} non è un consumabile!");
            }

            _item = itemInUse;
            _hitPointsToHeal = _item.HitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            ReportResult($"{actor.Name} si cura di {_hitPointsToHeal} punt{(_hitPointsToHeal > 1 ? "i" : "o")} salute!", GameMessageTypeEnum.BattleInfo);
            target.Heal(_hitPointsToHeal);
        }
    }
}
