using Classes;
using Interfaces;
using Player;
using Static;

namespace ActionCode
{
    public class Action0004 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerAttack>(out var controller)) return;
            controller.EnergyBall(Constants.SpellElements.Fire);
        }

        public void ActFinished()
        {
        }
    }
}