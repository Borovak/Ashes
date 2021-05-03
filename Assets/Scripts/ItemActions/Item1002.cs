using Player;
using Static;

namespace ItemActions
{
    public class Item1002: IItemAction
    {
        public void Use()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var lifeController)) return;
            lifeController.Heal(12f);
        }
    }
}