using Interfaces;
using Player;
using Static;

namespace ItemCode
{
    public class Item1000 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var controller)) return;
            controller.Heal(3f);
        }

        public void ActFinished()
        {
        }
    }
}