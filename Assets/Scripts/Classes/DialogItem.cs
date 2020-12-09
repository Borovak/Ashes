using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogItem
{
    public Sprite npcSprite;
    public string npcName;
    public string text;
    public Action ActionOnOk;
    public Dictionary<string, DialogItem> followUp;
}