using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;
using System;

namespace Engine.Actions
{
    public class AttackWithWeapon : IAction
    {
        private readonly Weapon _currentWeapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        public event EventHandler<GameMessageEventArgs> OnActionPerformed;

        public AttackWithWeapon(Weapon Weapon)
        {
            if (Weapon == null)
            {
                throw new ArgumentException($"{Weapon.Name} is not a weapon");
            }

            if (_minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }

            if (_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }

            _currentWeapon = Weapon;
            _minimumDamage = Weapon.MinimumDamage;
            _maximumDamage = Weapon.MaximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage;

            ReportResult($"{actor.Name} attacca {target.Name} con {_currentWeapon.Name}.", GameMessageType.BattleInfo);
            if (_currentWeapon.MissRate > BaseRandomNumberGenerator.NumberBetween(0, 100))
            {
                ReportResult($"{actor.Name} ha mancato {target.Name}!", (actor is Player)? GameMessageType.BattleNegative : GameMessageType.BattlePositive);
            }
            else
            {
                damage = BaseRandomNumberGenerator.NumberBetween(_currentWeapon.MinimumDamage, _currentWeapon.MaximumDamage);
                ReportResult($"{actor.Name} ha colpito {target.Name} causando {damage} danni!", (actor is Player) ? GameMessageType.BattlePositive : GameMessageType.BattleNegative);
                target.TakeDamage(damage);
            }
        }

        private void ReportResult(string result, GameMessageType type)
        {
            OnActionPerformed?.Invoke(this, new GameMessageEventArgs(result, type));
        }
    }
}
