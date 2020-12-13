using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public class WorldFactory
    {
        public World CreateWorld()
        {
            World NewWorld = new World();

            NewWorld.WorldID = 0;
            NewWorld.AddLocation(0, 0, "Home", "Casa Dolce Casa");
            NewWorld.AddLocation(0, 1, "Home01", "Casa Dolce Casa01");
            NewWorld.AddLocation(1, 0, "Home10", "Casa Dolce Casa10");
            NewWorld.AddLocation(1, 1, "Home11", "Casa Dolce Casa11");

            return NewWorld;
        }
    }
}
