using System;

[Serializable]
public static class Constants
{    
    [Serializable]
    public enum InteractionTypes
    {
        None,
        SavePoint,
        Shop,
        Talk
    }

    [Serializable]
    public enum Npc
    {
        None,
        Shopkeeper,
        HoboTom
    }
    
    public const int MONEY_ID = 0;
}