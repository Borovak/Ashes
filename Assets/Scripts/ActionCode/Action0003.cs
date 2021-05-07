using Interfaces;
using Player;
using Static;

namespace ActionCode
{
    public class Action0003 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerAttack>(out var controller)) return;
            controller.Heal();
        }

        public void ActFinished()
        {
        }
    }
}