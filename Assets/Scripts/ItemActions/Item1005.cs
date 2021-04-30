using Player;
using Static;

namespace ItemActions
{
    public class Item1005: IItemAction
    {
        public void Use()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<ManaController>(out var manaController)) return;
            manaController.Gain(10);
        }
    }
}