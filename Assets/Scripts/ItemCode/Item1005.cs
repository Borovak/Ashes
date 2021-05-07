using Interfaces;
using Static;

namespace ItemCode
{
    public class Item1005 : IAction
    {
        public void Act()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<ManaController>(out var controller)) return;
            controller.Gain(10f);
        }

        public void ActFinished()
        {
        }
    }
}