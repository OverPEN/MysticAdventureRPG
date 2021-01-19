using Engine.Models;

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
