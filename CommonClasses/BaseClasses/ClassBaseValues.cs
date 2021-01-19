using CommonClasses.Enums;

namespace CommonClasses.BaseClasses
{
    public class ClassBaseValues
    {
        public int HitPoints { get; set; }
        public int BaseDamage { get; set; }
        public WeaponDamageTypeEnum BaseDamageType { get; set; }
        public float Speed { get; set; }
        public int Gold { get; set; }

        public ClassBaseValues(PlayerClassTypeEnum _classType)
        {
            switch(_classType)
            {
                case PlayerClassTypeEnum.Trader:
                    HitPoints = int.MaxValue;
                    BaseDamage = 0;
                    BaseDamageType = WeaponDamageTypeEnum.Schianto;
                    Speed = 1.0f;
                    Gold = int.MaxValue;
                    break;

                case PlayerClassTypeEnum.Guerriero:
                    HitPoints = 35;
                    BaseDamage = 7;
                    BaseDamageType = WeaponDamageTypeEnum.Schianto;
                    Speed = 1.0f;
                    Gold = 200;
                break;

                case PlayerClassTypeEnum.Mago:
                    HitPoints = 20;
                    BaseDamage = 7;
                    BaseDamageType = WeaponDamageTypeEnum.Schianto;
                    Speed = 1.5f;
                    Gold = 150;
                break;

                case PlayerClassTypeEnum.Tank:
                    HitPoints = 50;
                    BaseDamage = 10;
                    BaseDamageType = WeaponDamageTypeEnum.Schianto;
                    Speed = 0.5f;
                    Gold = 250;
                break;

                case PlayerClassTypeEnum.Ladro:
                    HitPoints = 25;
                    BaseDamage = 5;
                    BaseDamageType = WeaponDamageTypeEnum.Schianto;
                    Speed = 2.0f;
                    Gold = 100;
                break;
            }
        }
    }
}
