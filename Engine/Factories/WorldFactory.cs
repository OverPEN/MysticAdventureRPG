using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class WorldFactory
    {
        public static World CreateWorld()
        {
            World NewWorld = new World();

            NewWorld.WorldID = 0;
            NewWorld.AddLocation(1,0, 0, "Home", "Casa Dolce Casa");
            NewWorld.AddLocation(2,0, 1, "Prato", "Un prato con erba alta");
            NewWorld.LocationAt(0, 1).AddQuestToLocation(1);
            NewWorld.LocationAt(0, 1).AddEnemyToLocation(1);
            NewWorld.AddLocation(3,1, 0, "Location", "Default");
            NewWorld.AddLocation(4,1, 1, "Location", "Default");

            return NewWorld;
        }
    }
}
