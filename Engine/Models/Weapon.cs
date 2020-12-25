using CommonClasses.Enums;
using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : Item
    {
        #region Public Properties
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public WeaponDamageType DamageType { get; }
        public float WeaponSpeed { get; }
        public int MissRate { get; }
        #endregion

        public Weapon(int itemID, string name, int price, int minDamage, int maxDamage, WeaponDamageType damageType, float weaponSpeed, int missRate, ItemType type = ItemType.Arma, byte quantity = 1, IAction action = null)
            : base(itemID, name, price, type = ItemType.Arma, quantity,1, true, action)
        {
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            DamageType = damageType;
            WeaponSpeed = weaponSpeed;
            MissRate = missRate;
        }

        #region Functions
        public new Weapon Clone(byte quantity = 1)
        {
            Weapon clonedWeapon = new Weapon(ItemID, Name, Price, MinimumDamage, MaximumDamage, DamageType, WeaponSpeed, MissRate, ItemType.Arma, quantity, Action);
            clonedWeapon.Action = new AttackWithWeapon(clonedWeapon);

            return clonedWeapon;
        }
        #endregion
    }
}
