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
        public int BaseDamage { get; set; }
        public int ActualDamage { get; set; }
        public WeaponDamageType DamageType { get; set; }
        public float WeaponSpeed { get; set; }

        public Weapon(int itemID, string name, int price, int baseDamage, int actualDamage, WeaponDamageType damageType, float weaponSpeed, ItemType type = ItemType.Arma)
            : base(itemID, name, price, type = ItemType.Arma)
        {
            BaseDamage = baseDamage;
            ActualDamage = actualDamage;
            DamageType = damageType;
            WeaponSpeed = weaponSpeed;
        }

        public new Weapon Clone()
        {
            return new Weapon(ItemID, Name, Price, BaseDamage, ActualDamage, DamageType, WeaponSpeed);
        }
    }
}
