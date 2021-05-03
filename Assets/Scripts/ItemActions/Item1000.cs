using Player;
using Static;

namespace ItemActions
{
    public class Item1000 : IItemAction
    {
        public void Use()
        {
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var lifeController)) return;
            lifeController.Heal(3f);
        }
    }
}