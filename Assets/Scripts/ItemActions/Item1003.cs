using Player;
using Static;

namespace ItemActions
{
    public class Item1003: IItemAction
    {
        public void Use()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var lifeController)) return;
            lifeController.Heal(30f);
        }
    }
}