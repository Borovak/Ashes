using Classes;
using Interfaces;
using Player;
using Static;

namespace ActionCode
{
    public class Action0006 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerAttack>(out var controller)) return;
            controller.EnergyBall(Constants.SpellElements.Electric);
        }

        public void ActFinished()
        {
        }
    }
}