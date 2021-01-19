using CommonClasses.Enums;
using Engine.Actions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Weapon : Item
    {
        #region Public Properties
        [JsonIgnore]
        public int MinimumDamage { get; set; }
        [JsonIgnore]
        public int MaximumDamage { get; set; }
        [JsonIgnore]
        public WeaponDamageTypeEnum DamageType { get; }
        [JsonIgnore]
        public float WeaponSpeed { get; }
        [JsonIgnore]
        public int MissRate { get; }
        #endregion

        public Weapon(int itemID, string name, int price, int minDamage, int maxDamage, WeaponDamageTypeEnum damageType, float weaponSpeed, int missRate, ItemTypeEnum type = ItemTypeEnum.Weapon, IAction action = null, List<PlayerClassTypeEnum> usableBy = null)
            : base(itemID, name, price, type = ItemTypeEnum.Weapon, true, action, usableBy)
        {
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
            DamageType = damageType;
            WeaponSpeed = weaponSpeed;
            MissRate = missRate;
        }

        #region Functions
        public new Weapon Clone()
        {
            Weapon clonedWeapon = new Weapon(ItemID, Name, Price, MinimumDamage, MaximumDamage, DamageType, WeaponSpeed, MissRate, ItemTypeEnum.Weapon, Action, UsableBy);
            clonedWeapon.Action = new AttackWithWeapon(clonedWeapon);

            return clonedWeapon;
        }
        #endregion
    }
}
