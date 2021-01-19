using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SaveState
    {
        public Player Player { get; set; }
        public World World { get; set; }

        public SaveState(Player player, World world)
        {
            Player = player;
            World = world;
        }
    }
}
