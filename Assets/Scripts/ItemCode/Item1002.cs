using Interfaces;
using Player;
using Static;

namespace ItemCode
{
    public class Item1002 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var controller)) return;
            controller.Heal(12f);
        }

        public void ActFinished()
        {
        }
    }
}