using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
