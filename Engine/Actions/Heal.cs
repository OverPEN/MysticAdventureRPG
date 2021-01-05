using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly Item _item;
        public readonly int _hitPointsToHeal;

        public Heal(Item itemInUse, int hitPointsToHeal) : base (itemInUse)
        {
            if (itemInUse.Type != ItemType.Consumabile)
            {
                throw new ArgumentException($"{itemInUse.Name} non è un consumabile!");
            }

            _item = itemInUse;
            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            ReportResult($"{actor.Name} si cura di {_hitPointsToHeal} punt{(_hitPointsToHeal > 1 ? "i" : "o")} salute!", GameMessageType.BattleInfo);
            target.Heal(_hitPointsToHeal);
        }
    }
}
