using Interfaces;
using Player;
using Static;

namespace ItemCode
{
    public class Item1003 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var controller)) return;
            controller.Heal(30f);
        }

        public void ActFinished()
        {
        }
    }
}