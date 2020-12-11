using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogItem
{
    Sprite npcSprite {get;}
    string npcName {get;}
    string text {get;}
    Action ActionOnOk {get;}
    Dictionary<string, IDialogItem> followUp {get;}
}