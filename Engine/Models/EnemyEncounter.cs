﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class EnemyEncounter
    {
        public int EnemyID { get; set; }
        public int EncounterRate { get; set; }

        public EnemyEncounter(int enemyID, int encounterRate)
        {
            EnemyID = enemyID;
            EncounterRate = encounterRate;
        }
    }
}
