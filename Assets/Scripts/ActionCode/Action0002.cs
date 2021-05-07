using Interfaces;
using Player;
using Static;

namespace ActionCode
{
    public class Action0002 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<ShieldController>(out var controller)) return;
            controller.shieldWantedState = true;
        }
        
        public void ActFinished()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<ShieldController>(out var controller)) return;
            controller.shieldWantedState = false;
        }
    }
}