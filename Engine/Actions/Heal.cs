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
    public class Heal : IAction
    {
        private readonly Item _item;
        public readonly int _hitPointsToHeal;

        public event EventHandler<GameMessageEventArgs> OnActionPerformed;

        public Heal(Item item, int hitPointsToHeal)
        {
            if (item.Type != ItemType.Consumabile)
            {
                throw new ArgumentException($"{item.Name} non è un consumabile!");
            }

            _item = item;
            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            ReportResult($"{actor.Name} si cura di {_hitPointsToHeal} punt{(_hitPointsToHeal > 1 ? "i" : "o")} salute!", GameMessageType.BattleInfo);
            target.Heal(_hitPointsToHeal);
        }

        private void ReportResult(string result, GameMessageType type)
        {
            OnActionPerformed?.Invoke(this, new GameMessageEventArgs(result, type));
        }
    }
}
