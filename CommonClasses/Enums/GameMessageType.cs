using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses.Enums
{
    public enum GameMessageType : byte
    {
        Info = 0,
        Encounter = 1,
        DamageSuffer = 2,
        DamageDeal = 3
    }
}
