using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class PlayerClassDefaultValues
    {
        public int HitPoints { get; set; }
        public int BaseDamage { get; set; }
        public DamageType BaseDamageType { get; set; }
        public float Speed { get; set; }
        public int Gold { get; set; }

        public PlayerClassDefaultValues(PlayerClassType _classType)
        {
            switch(_classType)
            {
                case PlayerClassType.Guerriero:
                    HitPoints = 35;
                    BaseDamage = 7;
                    BaseDamageType = DamageType.Taglio;
                    Speed = 1.0f;
                    Gold = 200;
                break;

                case PlayerClassType.Mago:
                    HitPoints = 20;
                    BaseDamage = 7;
                    BaseDamageType = DamageType.Magico;
                    Speed = 1.5f;
                    Gold = 150;
                break;

                case PlayerClassType.Tank:
                    HitPoints = 50;
                    BaseDamage = 10;
                    BaseDamageType = DamageType.Schianto;
                    Speed = 0.5f;
                    Gold = 250;
                break;

                case PlayerClassType.Ladro:
                    HitPoints = 25;
                    BaseDamage = 5;
                    BaseDamageType = DamageType.Penetrante;
                    Speed = 2.0f;
                    Gold = 100;
                break;
            }
        }
    }
}
