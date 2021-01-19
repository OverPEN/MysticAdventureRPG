using CommonClasses.Enums;

namespace CommonClasses.EventArgs
{
    public class GameMessageEventArgs : System.EventArgs
    {
        public string Message { get; private set; }
        public GameMessageTypeEnum Type { get; private set; }

        public GameMessageEventArgs(string message, GameMessageTypeEnum type)
        {
            Message = message;
            Type = type;
        }
    }
}
