using UnityEngine;

namespace Interfaces
{
    public interface IDialogItem
    {
        Sprite npcSprite { get; }
        string npcName { get; }
        string text { get; }
    }
}