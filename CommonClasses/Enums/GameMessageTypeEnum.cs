﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses.Enums
{
    public enum GameMessageTypeEnum : byte
    {
        Info = 0,
        BattleNegative = 1,
        BattlePositive = 2,
        BattleInfo = 3,
        ImportantInfo = 4
    }
}