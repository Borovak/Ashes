using Interfaces;
using Player;
using Static;

namespace ItemCode
{
    public class Item1001 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var controller)) return;
            controller.Heal(6f);
        }

        public void ActFinished()
        {
        }
    }
}