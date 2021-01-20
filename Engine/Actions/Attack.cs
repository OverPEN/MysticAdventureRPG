using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Models;
using System;

namespace Engine.Actions
{
    public class Attack : BaseAction, IAction
    {
        private readonly Weapon _currentWeapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        public Attack(Weapon itemInUse) : base (itemInUse)
        {
            if (itemInUse == null)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            if (itemInUse.MinimumDamage < 0)
            {
                throw new ArgumentException("Il danno minimo deve essere 0 o maggiore");
            }

            if (itemInUse.MaximumDamage < itemInUse.MinimumDamage)
            {
                throw new ArgumentException("Il danno massimo deve essere inferiore al danno minimo");
            }

            _currentWeapon = itemInUse;
            _minimumDamage = itemInUse.MinimumDamage;
            _maximumDamage = itemInUse.MaximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target, object attackType = null)
        {
            int damage;
            int cost = (_currentWeapon.MaximumDamage / 5) *2;

            ReportResult($"{actor.Name} attacca {target.Name} con {_currentWeapon.Name}.", GameMessageTypeEnum.BattleInfo);
            if (_currentWeapon.MissRate > BaseRandomNumberGenerator.NumberBetween(0, 100))
            {
                ReportResult($"{actor.Name} ha mancato {target.Name}!", (actor is Player)? GameMessageTypeEnum.BattleNegative : GameMessageTypeEnum.BattlePositive);
            }
            else
            {
                damage = BaseRandomNumberGenerator.NumberBetween(_currentWeapon.MinimumDamage, _currentWeapon.MaximumDamage);
                if (attackType.ToString() == "H")
                {
                    if ((actor.Class != PlayerClassTypeEnum.Mago && actor.CurrentStamina > cost) || actor.Class == PlayerClassTypeEnum.Mago && actor.CurrentMana > cost)
                    {
                        damage += _currentWeapon.MinimumDamage;
                        actor.LoseStaminaOrMana(cost);

                        ReportResult($"{actor.Name} ha consumato {cost} {(actor.Class != PlayerClassTypeEnum.Mago ? "Stamina" : "Mana")} ha colpito {target.Name} con un attacco potente causando {damage} dann{(damage > 1 ? "i" : "o")}!", (actor is Player) ? GameMessageTypeEnum.BattlePositive : GameMessageTypeEnum.BattleNegative);
                    }
                    else
                        ReportResult($"{actor.Name} non ha sufficente {(actor.Class != PlayerClassTypeEnum.Mago ? "Stamina" : "Mana")} per colpire con un attacco potente, e causa solo {damage} dann{(damage > 1 ? "i" : "o")}! {(actor.Class != PlayerClassTypeEnum.Mago ? "Stamina" : "Mana")} mancante: {(actor.Class != PlayerClassTypeEnum.Mago ? cost - actor.CurrentStamina : cost - actor.CurrentMana)}", (actor is Player) ? GameMessageTypeEnum.BattlePositive : GameMessageTypeEnum.BattleNegative);
                }
                else    
                    ReportResult($"{actor.Name} ha colpito {target.Name} causando {damage} dann{(damage > 1 ? "i" : "o")}!", (actor is Player) ? GameMessageTypeEnum.BattlePositive : GameMessageTypeEnum.BattleNegative);

                target.TakeDamage(damage);
            }
        }
    }
}
