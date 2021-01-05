using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly Item _itemInUse;

        public event EventHandler<GameMessageEventArgs> OnActionPerformed;

        protected BaseAction(Item itemInUse)
        {
            _itemInUse = itemInUse;
        }

        protected void ReportResult(string result, GameMessageType type)
        {
            OnActionPerformed?.Invoke(this, new GameMessageEventArgs(result, type));
        }
    }
}
