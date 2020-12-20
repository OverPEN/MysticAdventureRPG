using CommonClasses.EventArgs;
using Engine.Models;
using System;

namespace Engine.Actions
{
    public interface IAction
    {
        event EventHandler<GameMessageEventArgs> OnActionPerformed;
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
