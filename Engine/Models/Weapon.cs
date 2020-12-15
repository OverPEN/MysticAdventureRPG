using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : Item
    {
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public WeaponDamageType DamageType { get; set; }
        public float WeaponSpeed { get; set; }
        public int MissRate { get; set; }

        public Weapon(int itemID, string name, int price, int minDamage, int maxDamage, WeaponDamageType damageType, float weaponSpeed, int missRate, ItemType type = ItemType.Arma)
            : base(itemID, name, price, type = ItemType.Arma)
        {
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            DamageType = damageType;
            WeaponSpeed = weaponSpeed;
            MissRate = missRate;
        }

        public new Weapon Clone()
        {
            return new Weapon(ItemID, Name, Price, MinimumDamage, MaximumDamage, DamageType, WeaponSpeed, MissRate);
        }
    }
}
