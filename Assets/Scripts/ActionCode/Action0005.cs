using Classes;
using Interfaces;
using Player;
using Static;

namespace ActionCode
{
    public class Action0005 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerAttack>(out var controller)) return;
            controller.EnergySwipe(Constants.SpellElements.Fire);
        }

        public void ActFinished()
        {
        }
    }
}