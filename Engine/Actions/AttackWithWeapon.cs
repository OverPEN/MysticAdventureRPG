﻿using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;
using System;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly Weapon _currentWeapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        public AttackWithWeapon(Weapon itemInUse) : base (itemInUse)
        {
            if (itemInUse == null)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            if (_minimumDamage < 0)
            {
                throw new ArgumentException("Il danno minimo deve essere 0 o maggiore");
            }

            if (_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("Il danno massimo deve essere inferiore al danno minimo");
            }

            _currentWeapon = itemInUse;
            _minimumDamage = itemInUse.MinimumDamage;
            _maximumDamage = itemInUse.MaximumDamage;
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
    }
}
