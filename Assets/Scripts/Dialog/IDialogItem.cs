using UnityEngine;

namespace Dialog
{
    public interface IDialogItem
    {
        Sprite NpcSprite { get; }
        string NpcName { get; }
        string Text { get; }
    }
}